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

	//acknowledgement
	pm.RegisterPacketDataType(&packet.PacketDataType{
		Description: "Acknowledgement of Success or Raising Error",
		ID:          0,
		Primitives:  []kind{reflect.String, reflect.Uint16},
	})
	return pm
}

//region Writing Utils

func (p *player) WriteMessage(message string) {
	pac := packet.NewPacketWithStrings(16, []string{message})
	p.clientInstance.writeChannel <- pac
}

func (p *player) WritePacketAcknowledgeOrError(id uint16, err string) {
	pac := packet.NewPacketWithStrings(0, []string{err})
	pac.WriteUInt16(id)
	p.clientInstance.writeChannel <- pac
}

//endregion
