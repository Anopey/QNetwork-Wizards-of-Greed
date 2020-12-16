package main

import "net"

var idToPlayer map[int]*player
var players []*player

type client struct {
	conn *net.Conn
}

type player struct {
	clientInstance *client
	username       string
	id             int
}

func initializeClientManagementParams() {
	idToPlayer = make(map[int]*player)
	for i := 0; i < totalPlayerLimit; i++ {
		idToPlayer[i] = nil
	}
	players = make([]*player, 0, totalPlayerLimit)
}

func newPlayer(id int, conn *net.Conn) *player {
	var newClient = client{
		conn: conn,
	}
	var newPlayer = player{
		clientInstance: &newClient,
		id:             id,
		username:       "",
	}

	idToPlayer[id] = &newPlayer
	players = append(players, &newPlayer)
	return &newPlayer
}

func constructBasePlayerIfValid(conn *net.Conn) *player {
	id := -1

	//try to find valid id TODO: Optimize
	for i := 0; i < totalPlayerLimit; i++ {
		if idToPlayer[i] == nil {
			id = i
		}
	}

	if id == -1 {
		//no valid id could be found! Server is over limit.
		return nil
	}

	return newPlayer(id, conn)
}