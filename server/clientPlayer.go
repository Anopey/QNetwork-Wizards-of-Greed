package main

import (
	"net"

	"github.com/Anopey/Appease/server/packet"
)

var idToPlayer map[uint32]*player

type client struct {
	conn            *net.Conn
	writeChannel    chan *packet.Packet
	clientTerminate chan interface{}
}

type player struct {
	clientInstance *client
	username       string
	id             uint32
	ableToPlay     bool
	active         bool
}

func initializeClientManagementParams() {
	idToPlayer = make(map[uint32]*player)
	for i := uint32(0); i < totalPlayerLimit; i++ {
		idToPlayer[i] = nil
	}
}

func newPlayer(id uint32, conn *net.Conn) *player {
	var newClient = client{
		conn:            conn,
		writeChannel:    make(chan *packet.Packet, 5),
		clientTerminate: make(chan interface{}),
	}
	var newPlayer = player{
		clientInstance: &newClient,
		id:             id,
		username:       "",
		active:         true,
	}

	idToPlayer[id] = &newPlayer
	return &newPlayer
}

func constructBasePlayerIfValid(conn *net.Conn) *player {
	id := ^uint32(0)

	//try to find valid id TODO: Optimize
	for i := uint32(0); i < totalPlayerLimit; i++ {
		if idToPlayer[i] == nil {
			id = i
		}
	}

	if id == ^uint32(0) {
		//no valid id could be found! Server is over limit.
		return nil
	}

	return newPlayer(id, conn)
}

func removePlayer(p *player) {
	_, ok := idToPlayer[p.id]
	if ok {
		delete(idToPlayer, p.id) //TODO: ensure that this does not raise issues when it comes to concurrency.
	}
}
