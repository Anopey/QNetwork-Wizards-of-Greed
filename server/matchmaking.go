package main

import (
	"sync"
)

const (
	maximumPlayersInQueue int = 8
)

type playerQueueInfo struct {
	isReady    bool
	queueIndex uint16
}

var playersInQueue []*player = make([]*player, 8)
var queueReadyCount uint16 = 0

var queueMutex sync.RWMutex = sync.RWMutex{}

func getMatchmakingCommonErrors(p *player) string {
	if !p.ableToPlay {
		return "Player's isAbleToPlay field is set to false!"
	}

	if p.activeGame != nil {
		return "Player is already in a game!"
	}

	return ""
}

func leaveQueue(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if p.queueInfo == nil {
		return "The player is not queued up!"
	}

	//leave queue logic

	if p.queueInfo.isReady {
		queueReadyCount--
	}

	index := p.queueInfo.queueIndex

	playersInQueue[index] = playersInQueue[len(playersInQueue)-1]

	playersInQueue = playersInQueue[:len(playersInQueue)-1]

	playersInQueue[index].queueInfo.queueIndex = index //assign leaving player's index to the new occupant of his/her index.

	p.queueInfo = nil

	writeAllQueueInfo()

	queueMutex.Unlock()

	return ""
}

func queueUp(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if p.queueInfo != nil {
		return "The player is already queued up!"
	}

	if len(playersInQueue) == maximumPlayersInQueue {
		return "The queue is full!"
	}

	//queue up logic

	p.queueInfo = &playerQueueInfo{
		isReady:    false,
		queueIndex: uint16(len(playersInQueue)),
	}

	playersInQueue = append(playersInQueue, p)

	writeAllQueueInfo()

	queueMutex.Unlock()

	return ""
}

func readyUp(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if p.queueInfo == nil {
		return "The player is not queued up!"
	}

	writeAllQueueInfo()

	queueMutex.Unlock()

	return ""
}

func unready(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if p.queueInfo == nil {
		return "The player is not queued up!"
	}

	writeAllQueueInfo()

	queueMutex.Unlock()

	return ""
}

//later on we will need faster ways when we have a fully fledged matchmaking system.

func writeAllQueueInfo() {
	for _, p := range playersInQueue {
		p.WriteQueueInfo(uint16(len(playersInQueue)), queueReadyCount)
	}
}
