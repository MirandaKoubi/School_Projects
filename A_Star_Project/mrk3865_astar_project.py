#Name:			Miranda Koubi
#CLID:			mrk3865
#Class:			CMPS 420	Spring 2015
#Due Date:		February 20, 2015 at 10:00 am
#Project:		#1 8-Puzzle Solver using A* Algorithm
#Assignment:
#Implement a program that solves an 8-puzzle using the A* shortest path algorithm.

import heapq
import time

startInput = " "
goalInput = " "

class Board:
	def __init__(self, state, parent):
		#self.state equals an empty 3x3 array
		self.state = [[0 for x in range (3)] for x in range (3)]
		
		#fills the self.state array with the values passed in from state
		for i in range(3):
			for j in range(3):
				self.state[i][j] = state[i][j]
		
		self.manhattan()
		self.setParent(parent)
		self.name = self.toString()
	
	#converts states to strings for comparisons
	def toString(self):
	
		string = ""
	
		for i in range(3):
			for j in range(3):
				string += self.state[i][j]
	
		return string
		
	def setParent(self, parent):

		self.g = 0
		self.f = 0
		self.parent = parent
		
		#sets the g value of the current state to the g value of its parent + 1
		if parent:
			self.g = parent.g + 1
		
		#the f value is the g value plus the h value
		self.f = self.h + self.g
		
	#calculate the distance from where a tile is to where it should be
	def manhattan(self):
	
		distance = 0
		for i in range(3):
			for j in range(3):
				if goal[i][j] != "0":
					#get location of where that tile ([i][j]) should be in the goal state
					location = self.getTile(goal[i][j])
					distance += abs(location[0] - i) + abs(location[1] - j)
					
		self.h = distance
		
	#takes a tile and gets where it is on the board			
	def getTile(self, tile):
		#iterates through self.state and finds where tile is located
		for i in range(3):
			for j in range(3):
				if self.state[i][j] == tile:
					return (i,j)
					
	def __eq__(self, other):
		#does this board position equal other
		#comparison of two states
		#overlaod equals operator
	
		return self.name == other.name
		
	def __ne__(self, other):
		#does this board position not equal other?
		#overload not equals operator
		return not self == other
		
	def __lt__(self, other):
		return self.f < other.f
	
	def __gt__(self, other):
		return self.f > other.f
	
	def __le__(self, other):
		return self.f <= other.f
	
	def __ge__(self, other):
		return self.f >= other.f
	
	def getNeighbors(self):
	
		neighbors = []
		
		#get tuple where 0 is located
		spaceJam = self.getTile("0")
		
		a = spaceJam[0]
		b = spaceJam[1]
		
		if spaceJam[0] > 0:
			#create an empty matrix for neighborState
			neighborState = [[0 for x in range (3)] for x in range (3)]
			
			#fill the neighborState will indicies from self.state
			for i in range(3):
				for j in range(3):
					neighborState[i][j] = self.state[i][j]
			
			#swicth the space's position from where it is in self.state to one to the left in neighbior
			temp = neighborState[a][b]
			neighborState[a][b] = neighborState[a-1][b]
			neighborState[a-1][b] = temp
			
			#create a new board out of this neighbor
			newNeighbor = Board(neighborState, self)
			
			#add the neighbor to the list of possible neighbors
			neighbors.append(newNeighbor)	
			
		if spaceJam[0] < 2:
			#create an empty matrix for neighborState
			neighborState = [[0 for x in range (3)] for x in range (3)]
			
			#fill the neighborState will indicies from self.state
			for i in range(3):
				for j in range(3):
					neighborState[i][j] = self.state[i][j]
			
			#swicth the space's position from where it is in self.state to one to the right in neighbior
			temp = neighborState[a][b]
			neighborState[a][b] = neighborState[a+1][b]
			neighborState[a+1][b] = temp
			
			#create a new board out of this neighbor
			newNeighbor = Board(neighborState, self)
			
			#add the neighbor to the list of possible neighbors
			neighbors.append(newNeighbor)
			
		if spaceJam[1] > 0:
			#create an empty matrix for neighborState
			neighborState = [[0 for x in range (3)] for x in range (3)]
			
			#fill the neighborState will indicies from self.state
			for i in range(3):
				for j in range(3):
					neighborState[i][j] = self.state[i][j]
			
			#swicth the space's position from where it is in self.state to one below in neighbior
			temp = neighborState[a][b]
			neighborState[a][b] = neighborState[a][b-1]
			neighborState[a][b-1] = temp
			
			#create a new board out of this neighbor
			newNeighbor = Board(neighborState, self)
			
			#add the neighbor to the list of possible neighbors
			neighbors.append(newNeighbor)
			
		if spaceJam[1] < 2:
			#create an empty matrix for neighborState
			neighborState = [[0 for x in range (3)] for x in range (3)]
			
			#fill the neighborState will indicies from self.state
			for i in range(3):
				for j in range(3):
					neighborState[i][j] = self.state[i][j]
			
			#swicth the space's position from where it is in self.state to one to the left in neighbior
			temp = neighborState[a][b]
			neighborState[a][b] = neighborState[a][b+1]
			neighborState[a][b+1] = temp
			
			#create a new board out of this neighbor
			newNeighbor = Board(neighborState, self)
			
			#add the neighbor to the list of possible neighbors
			neighbors.append(newNeighbor)
	
		#return list of neighbors
		return neighbors
		
	def printBoard(self):
		#print board as a matrix
		for row in self.state:
			for column in row:
				print(column if column != "0" else " ", end=" ")
			print(end="\n")
			
		print(" ")
		
def isSolvable(startInput, goalInput):
	#have list of start and list of goal
	#if what is before the current index in start is after the current index in goal
	#then it is an inversion 
	
	inversions = 0
	
	#turn the start and goal inputs into lists
	startList = list(startInput)
	goalList = list(goalInput)
	
	#remove 0 from both lists, since 0 is not counted when determining solvability
	goalList.remove("0")
	startList.remove("0")
	
	#loop through elements in list
	for i in range(8):
		goalIndex = goalList.index(startList[i])
		
		#splice the start list and goal lists from where i is in both
		startSplice = startList[0:i]
		goalSplice = goalList[goalIndex + 1:8]
		
		#whatever is in both the startSplice and goalSplice is an inversion and is added to inversions
		inversions += len(set(startSplice) & set(goalSplice))
		
	#if there are an even number of inversions, return True and the puzzle is solvable
	#if there are an odd number of inversions, return False and the puzzle is not solvable
	if (inversions % 2 == 0):
		return True
	else:
		return False

#A* algorithm		
def puzzleMaster():
	
	startTime = time.time()
	
	startState = Board(start, None)
	goalState = Board(goal, None)
	
	open = []
	closed = []
	path = []
	
	#puts the start state into the heap
	heapq.heappush(open, startState)
	count = 0
	
	while (open != []):
		currentState = heapq.heappop(open)
		
		if currentState == goalState:
			#return path from start to currentState 
			path.append(currentState)
			while currentState.parent:
				path.append(currentState.parent)
				currentState = currentState.parent
			
			path.reverse()

			for i in path:
				i.printBoard()
			
			print("Solution Found!")
			print(" ")
			print("Solution was found in " + str(count) + " state examinations.")
			print("Number of moves: " + str(len(path) - 1))
			print("Solution found in about " + str(int(time.time() - startTime)) + " seconds.")
			print(" ")
				
			return path
			
		else:
			children = currentState.getNeighbors()
			
			for child in children:
				if child in open:
					#if this state already exists in the open list
					#check the g values of the two states to see which one is shorter
					#if the current state's path is shorter, assign the shorter to open
					#assign open's parent to be the current state
					openState = open[open.index(child)]
					if currentState.g + 1 < openState.g:
						openState.setParent(currentState)
						open.sort()
					
				elif child in closed:
					# print("Found in closed!")
					closedIndex = closed.index(child)
					closedState = closed[closedIndex]
					if currentState.g + 1 < closedState.g:
						closed.pop(closedIndex)
						heapq.heappush(open, child)
				else:
					# print("Found in neither!")
					count += 1
					heapq.heappush(open, child)
							
			closed.append(currentState)
			
	return None

def main():

	print("This is a program that solves the 8-puzzle problem. The input is two strings of non-repeating numbers. One for the start state, and one for the goal state. 0 represents the open space in the puzzle.")
	print("example:012345678 is a possible start state, and 876543210 is a possible goal state. Not every puzzle will have a solution.")
	print(" ")
	correctInput = False
	correctStart = False
	correctGoal = False
	
	#checks to make sure input is valid
	while (correctInput != True):
		
		startInput = input("Start State (numbers 0-8, 0 for empty space): ")
		goalInput = input("Goal State (numbers 0-8, 0 for empty space): ")
	
		if startInput.isnumeric() == False or '0' not in startInput or '9' in startInput or len(startInput) != 9 or len(startInput) != len(set(startInput)):
			print ("Invalid Start State.")
		else:
			correctStart = True
			
		if goalInput.isnumeric() == False or '0' not in goalInput or '9' in goalInput or len(goalInput) != 9 or len(goalInput) != len(set(goalInput)):
			print ("Invalid Goal State.")
		else:
			correctGoal = True
			
		if correctStart == True and correctGoal == True:
			correctInput = True
		
	global start
	global goal
	
	#list(start)
	start = [[startInput[0], startInput[1], startInput[2]], [startInput[3], startInput[4], startInput[5]], [startInput[6], startInput[7], startInput[8]]]

	#list(goal)
	goal = [[goalInput[0], goalInput[1], goalInput[2]], [goalInput[3], goalInput[4], goalInput[5]], [goalInput[6], goalInput[7], goalInput[8]]]	
	
	#if the board given is valid and the puzzle is solvable, runs the puzzle solver
	if isSolvable(startInput, goalInput):	
		puzzleMaster()
	else:		
		print("Puzzle is not solvable.")
	
	input('Press ENTER to exit')
main()