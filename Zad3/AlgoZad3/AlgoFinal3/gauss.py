# TO BE COMPILED WITH PYTHON >= 3.0

import math
import random as rnd
from copy import deepcopy
from decimal import *
from typing import TypeVar, Generic

T = TypeVar('T')


class Matrix:
	def __init__(self, T, A, B):
		self.T = T
		self.n = A[0].size
		self.A = A
		self.X = [T(0) for i in range(self.n)]
		self.Xprim = [T(0) for i in range(self.n)]
		self.B = B
		self.C_matrix = [[T(0) for i in range(self.n + 1)] for j in range(self.n)]
		self.create_C_matrix(True)       
        

	def find_B(self, new_mults):
		n = self.n
		for i in range(n):
			for j in range(n):
				self.B[i] += self.A[i][j] * self.X[j]

		self.create_C_matrix(new_mults)

	def convert_types(self, T):
		n = self.n
		self.C_matrix = [[T(0) for i in range(self.n)] for j in range(self.n + 1)]
		m = len(self.C[0])
		newX = []
		newB = []

		if isinstance(self.A[0][0], Fraction):
			for i in range(n):
				for j in range(n):
					self.A[i][j] = self.A[i][j].decimal
				newX.append(self.X[i].decimal)
				newB.append(self.B[i].decimal)
		else:
			for i in range(n):
				for j in range(n):
					self.A[i][j] = T(self.A[i][j])
				newX.append(T(self.X[i]))
				newB.append(T(self.B[i]))
		self.X = newX
		self.B = newB
		self.find_B(False)

	def create_C_matrix(self, new_mults):
		for i in range(self.n):
			for j in range(self.n):
				self.C_matrix[i][j] = self.A[i][j]
		for i in range(self.n):
			self.C_matrix[i][self.n] = self.B[i]        
		return self.C_matrix

	def print_A(self):
		s = "MATRIX A\n"
		for i in range(self.n):
			for j in range(self.n):
				s += str(self.A[i][j]) + "   "
			s += "\n"
		print(s)

	def print_B(self):
		s = "MATRIX B\n"
		for i in range(self.n):
			s += str(self.B[i]) + "\n"
		print(s)

	def print_C(self):
		s = "MATRIX C\n"
		for i in range(self.n):
			for j in range(self.n + 1):
				s += str(self.C_matrix[i][j]) + "   "
			s += "\n"
		print(s)

	def print_X(self):
		s = "MATRIX X\n"
		for i in range(self.n):
			s += str(self.X[i]) + "   "
		s += "\n"
		print(s)

	def print_Xprim(self):
		s = "MATRIX X\n"
		for i in range(self.n):
			s += str(self.Xprim[i]) + "   "
		s += "\n"
		print(s)

	def print_Array(self, C):
		s = "MATRIX:\n"
		for i in range(self.n):
			for j in range(self.n + 1):
				s += str(C[i][j]) + "   "
			s += "\n"
		print(s)

	def print_All(self):
		self.print_A()
		self.print_B()
		self.print_C()
		self.print_X()

	def generate_r(self):
		# return self.T(rnd.randint(-65536, 65535) / 65536)
		return rnd.randint(1,5)

	def swap_rows(self, C, current_i, current_j):
		n = len(C)
		m = len(C[0])

		max_index = 0
		max1 = C[current_i][current_j]
		found = False
		for i in range(current_i, n):
			if abs(C[i][current_j]) > abs(max1):
				max1 = C[i][current_j]
				max_index = i
				found = True
		if found:
			for j in range(m):
				temp = C[current_i][j]
				C[current_i][j] = C[max_index][j]
				C[max_index][j] = temp
		return C

	def swap_cols(self, C, current_i, current_j):
		n = len(C)
		m = len(C[0])

		max_index = 0
		max1 = C[current_i][current_j]
		found = False
		for j in range(current_j, n):
			if abs(C[current_i][j]) > abs(max1):
				max1 = C[current_i][j]
				max_index = j
				found = True
		if found:
			for i in range(n):
				temp = C[i][current_j]
				C[i][current_j] = C[i][max_index]
				C[i][max_index] = temp
		return C

	def swap_all(self, C, current_i, current_j):
		C = self.swap_cols(C, current_i, current_j)
		C = self.swap_rows(C, current_i, current_j)
		return C

	def reduce(self, C, i):
		n = len(C)
		m = len(C[0])
		first = C[i][i]
		for k in range(m):
				C[i][k] = C[i][k] / first

		for j in range(i + 1, n):
			first = C[j][i]
			for k in range(m):
				C[j][k] -= C[i][k] * first
		return C

	def gauss(self):
		n = self.n
		C = deepcopy(self.C_matrix)
		Xprim = self.Xprim
		m = len(C[0])

		for i in range(n):
			if C[i][i] == 0:
				C = self.swap_rows(C, i, i)
			C = self.reduce(C, i)

		for i in range(n - 1, -1, -1):
			Xprim[i] = C[i][n] / C[i][i]
			for k in range(i - 1, -1, -1):
				C[k][n] -= C[k][i] * Xprim[i]
		self.print_Xprim()
		return Xprim

	def gauss_part(self):
		n = self.n
		C = deepcopy(self.C_matrix)
		Xprim = self.Xprim
		m = len(C[0])

		for i in range(n):
			C = self.swap_rows(C, i, i)
			if C[i][i] == 0:
				C = self.swap_rows(C, i, i)
			C = self.reduce(C, i)

		for i in range(n - 1, -1, -1):
			Xprim[i] = C[i][n] / C[i][i]
			for k in range(i - 1, -1, -1):
				C[k][n] -= C[k][i] * Xprim[i]

		self.print_Xprim()
		return Xprim

	def gauss_full(self):
		n = self.n
		C = deepcopy(self.C_matrix)
		Xprim = self.Xprim
		m = len(C[0])

		for i in range(n):
			C = self.swap_all(C, i, i)
			if C[i][i] == 0:
				C = self.swap_all(C, i, i)
			C = self.reduce(C, i)

		for i in range(n - 1, -1, -1):
			Xprim[i] = C[i][n] / C[i][i]
			for k in range(i - 1, -1, -1):
				C[k][n] -= C[k][i] * Xprim[i]

		self.print_Xprim()
		return Xprim