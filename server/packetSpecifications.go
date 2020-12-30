package main

import (
	"reflect"

	"github.com/Anopey/Appease/server/packet"
)

type kind = reflect.Kind

func initializePackets() *packet.PacketManager {
	pm := packet.GetPacketManager()

	//simple messages
	pm.RegisterPacketDataType(&packet.PacketDataType{
		Description: "Simple Message",
		ID:          16,
		Primitives:  []kind{reflect.String},
		//handler yes
	})
	return pm
}

//region Writing Utils

func (p *player) WriteMessage(message string) {
	pac := packet.NewPacketWithStrings(16, []string{message})
	p.clientInstance.writeChannel <- pac
}

//endregion
