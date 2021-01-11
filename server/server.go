package main

import (
	"fmt"
	"log"
	"net"
	"strconv"

	"github.com/Anopey/Appease/server/packet"
)

//region Server Properties

const (
	port             string = ":52515"
	totalPlayerLimit uint32 = 16
	dataBufferSize   uint16 = 4096
)

var serverIsActive bool

//ServerIsActive returns true if and only if the server is currently active and listening.
func ServerIsActive() bool {
	return serverIsActive
}

//endregion

//region Server Initialization and Listening

var packetManager *packet.PacketManager

func initializeServerParams() {
	initializeClientManagementParams()
	packetManager = initializePackets()
}

func main() {

	initializeServerParams()

	listener, err := net.Listen("tcp", port)

	if err != nil {
		fmt.Println(err)
		return
	}

	defer listener.Close()

	fmt.Println("Now listening on port " + port + "...")

	serverIsActive = true

	go delegateChannels()

	for serverIsActive {
		conn, err := listener.Accept()
		if err != nil {
			log.Fatalln(err.Error()) //in which cases is an error raised? is terminating entire server worth this?
		}

		fmt.Println("New connection from " + conn.RemoteAddr().String())
		go processInitialConnection(&conn)
	}
}

//endregion

//region Connection and Client/Player Construction

func processInitialConnection(conn *net.Conn) {
	p := constructBasePlayerIfValid(conn)
	if p == nil {
		fmt.Println(getCurrentServerTime() + ": " + "INVALID CONNECTION FROM: " + (*conn).RemoteAddr().String())
		(*conn).Close()
		return
	}
	startClientProcesses(p)
}

func startClientProcesses(p *player) {
	go tendToClientChannels(p)
	p.WriteMessage("Welcome!")
	tendToClientRead(p)
}

//endregion

//region Client Processing

func tendToClientRead(p *player) {
	buf := make([]byte, dataBufferSize)

	conn := p.clientInstance.conn

	for {
		len, err := (*conn).Read(buf)

		if err != nil {
			fmt.Printf("***\nError reading data from player" + "\n Server Time:" + getCurrentServerTime() + "\n ID:" + strconv.Itoa(int(p.id)) + "\nUsername: " + p.username +
				"\n IP:" + (*p.clientInstance.conn).RemoteAddr().String() + "\nError:" + err.Error() + "\n***\n")
			//TODO disconnect client etc.
			p.WriteMessage("Unable to read from client. Terminating Client.")
			p.clientInstance.clientTerminate <- struct{}{}
			break
		}

		//for now print as string.

		//TODO: Test TCP reading on server.

		_packet := packet.NewPacketByCopy(buf)

		val, ok := idToHandler[_packet.ID]

		if ok {
			//packet is handled

			go val(_packet, p)

		} else {
			//default echo behaviour
		}

		s := string(buf[:len])

		fmt.Println("Message", s)
		fmt.Println("Length", len)
	}
}

func tendToClientChannels(p *player) {
	conn := p.clientInstance.conn
	for {
		select {
		case pac := <-p.clientInstance.writeChannel:
			fmt.Printf("writing to player with ID %d", p.id)
			_, err := (*conn).Write(pac.GetAllBytes())
			if err != nil {
				fmt.Printf(err.Error())
			}
			break
		case <-p.clientInstance.clientTerminate:
			p.active = false
			p.ableToPlay = false
			if p.queueInfo != nil {
				leaveQueue(p)
			}
			(*p.clientInstance.conn).Close()
			removePlayer(p)
			return
		}
	}
}

//endregion

//region Delegate Channels and Server Control

func closeServer() {
	//handle all things that need to be shut down. send appropriate messages to all clients etc.
	serverIsActive = false
}

var serverCloseChannel = make(chan interface{})

func delegateChannels() {
	for serverIsActive {
		select {
		case <-serverCloseChannel:
			closeServer()
			break
		}
	}
}

//endregion
