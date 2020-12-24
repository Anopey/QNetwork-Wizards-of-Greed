package main

import (
	"reflect"

	"github.com/Anopey/Appease/server/packet"
)

type kind = reflect.Kind

func initializePackets() *packet.PacketManager {
	pm := packet.NewPacketManager()

	//simple messages
	pm.RegisterPacketDataType(&packet.PacketDataType{
		Description: "Simple Message",
		ID:          16,
		Primitives:  []kind{reflect.String},
		//handler yes
	})
	return pm
}


