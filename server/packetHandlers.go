package main

import (
	"fmt"

	"github.com/Anopey/Appease/server/packet"
)

type packetHandler func(*packet.Packet, *player)

var idToHandler map[uint16]packetHandler = map[uint16]packetHandler{
	16: packet16Handler,
	17: packet17UsernameHandler,
	18: packet18UsernameHandler,
}

//region Default

//msg
func packet16Handler(_packet *packet.Packet, _player *player) {
	fmt.Printf("Message from player with ID %d: \n %s", _player.id, _packet.ReadString())
}

func packet17UsernameHandler(_packet *packet.Packet, _player *player) {
	username := _packet.ReadString()
	fmt.Printf("Player with ID %d is setting their username to %s", _player.id, username)
	_player.username = username
	_player.ableToPlay = true
	_player.WritePacketAcknowledgeOrError(17, "")
}

func packet18UsernameHandler(_packet *packet.Packet, _player *player) {

}

//endregion
