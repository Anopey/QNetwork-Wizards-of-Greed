package main

import (
	"sync"
)

const (
	maximumPlayersInQueue int = 8
)

type playerQueueInfo struct {
	p       *player
	isReady bool
}

var playersInQueue []*playerQueueInfo = make([]*playerQueueInfo, 8)
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

	if !queueContainsPlayer(p) {
		return "The player is not queued up!"
	}

	//leave queue logic

	var index uint16 = uint16(maximumPlayersInQueue + 1)

	for i, c := range playersInQueue {
		if c.p == p {
			index = uint16(i)
			break
		}
	}

	if playersInQueue[index].isReady {
		queueReadyCount--
	}

	playersInQueue[index] = playersInQueue[len(playersInQueue)-1]

	playersInQueue = playersInQueue[:len(playersInQueue)-1]

	queueMutex.Unlock()

	return ""
}

func queueUp(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if len(playersInQueue) == maximumPlayersInQueue {
		return "The queue is full!"
	}

	if queueContainsPlayer(p) {
		return "The player is already queued up!"
	}

	//queue up logic

	playersInQueue = append(playersInQueue, &playerQueueInfo{
		p:       p,
		isReady: true,
	})

	queueMutex.Unlock()

	return ""
}

func readyUp(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if !queueContainsPlayer(p) {
		return "The player is not queued up!"
	}

	queueMutex.Unlock()

	return ""
}

func unready(p *player) string {

	err := getMatchmakingCommonErrors(p)

	if err != "" {
		return err
	}

	queueMutex.Lock()

	if !queueContainsPlayer(p) {
		return "The player is not queued up!"
	}

	queueMutex.Unlock()

	return ""
}

//later on we will need faster ways when we have a fully fledged matchmaking system.
func queueContainsPlayer(p *player) bool {
	for _, c := range playersInQueue {
		if c.p == p {
			return true
		}
	}
	return false
}
