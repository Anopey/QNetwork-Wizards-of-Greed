package main

import (
	"fmt"
	"log"
	"net"
)

//region Server Properties

const port string = "52515"

var serverIsActive bool

//ServerIsActive returns true if and only if the server is currently active and listening.
func ServerIsActive() bool {
	return serverIsActive
}

//endregion

func main() {
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
		go handleConnection(&conn)
	}
}

func handleConnection(conn *net.Conn) {

}

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
