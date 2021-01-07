package main

import (
	"fmt"

	"github.com/Anopey/Appease/server/packet"
)

type packetHandler func(*packet.Packet, *player)

var idToHandler map[uint16]packetHandler = map[uint16]packetHandler{}

//region Default

//msg
func packet16Handler(_packet *packet.Packet, _player *player) {
	fmt.Printf("Message from player with ID %d: \n %s", _player.id, _packet.ReadString())
}

//endregion
