package main

import (
	"fmt"
	"log"
	"net"
	"time"
)

//region Server Properties

const port string = "52515"
const totalPlayerLimit int = 16

var serverIsActive bool
var idToPlayer map[int]*player

//ServerIsActive returns true if and only if the server is currently active and listening.
func ServerIsActive() bool {
	return serverIsActive
}

//endregion

//region Type Declarations

type client struct {
	conn *net.Conn
}

type player struct {
	clientInstance *client
	username       string
	id             int
}

//endregion

//region Server Initialization and Listening

func initializeServerParams() {
	idToPlayer = make(map[int]*player)
	for i := 0; i < totalPlayerLimit; i++ {
		idToPlayer[i] = nil
	}
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
	p := constructBasePlayer(conn)
	if p == nil {
		fmt.Println(time.Now().Format("2006-01-02 15:04:05") + ": " + "INVALID CONNECTION FROM: " + (*conn).RemoteAddr().String())
		(*conn).Close()
		return
	}

}

func constructBasePlayer(conn *net.Conn) *player {
	id := -1

	//try to find valid id TODO: Optimize
	for i := 0; i < totalPlayerLimit; i++ {
		if idToPlayer[i] != nil {
			id = i
		}
	}

	if id == -1 {
		//no valid id could be found! Server is over limit.
		return nil
	}

	var newClient = client{
		conn: conn,
	}
	var newPlayer = player{
		clientInstance: &newClient,
		id:             id,
		username:       "",
	}

	return &newPlayer
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
