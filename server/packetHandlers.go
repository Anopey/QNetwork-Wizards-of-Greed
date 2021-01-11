package main

import (
	"fmt"

	"github.com/Anopey/Appease/server/packet"
)

type packetHandler func(*packet.Packet, *player)

var idToHandler map[uint16]packetHandler = map[uint16]packetHandler{
	16: packet16Handler,
	17: packet17UsernameHandler,
	18: packet18QueueUpHandler,
	19: packet19LeaveQueueHandler,
	20: packet20QueueReadyUnreadyHandler,
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

func packet18QueueUpHandler(_packet *packet.Packet, _player *player) {
	fmt.Printf("Player with ID %d is queuing up!", _player.id)
	err := queueUp(_player)
	(*_player).WritePacketAcknowledgeOrError(18, err)
}

func packet19LeaveQueueHandler(_packet *packet.Packet, _player *player) {
	fmt.Printf("Player with ID %d is leaving queue!", _player.id)
	err := leaveQueue(_player)
	(*_player).WritePacketAcknowledgeOrError(19, err)
}

func packet20QueueReadyUnreadyHandler(_packet *packet.Packet, _player *player) {
	ready := _packet.ReadBool()

	err := ""

	if ready {
		fmt.Printf("Player with ID %d is readying up in queue!", _player.id)
		err = readyUp(_player)
	} else {
		fmt.Printf("Player with ID %d is unreadying in queue!", _player.id)
		err = unready(_player)
	}
	(*_player).WritePacketAcknowledgeOrError(20, err)
}

//endregion
