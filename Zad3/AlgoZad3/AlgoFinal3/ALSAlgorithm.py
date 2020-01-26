import numpy as np
from typing import List
from objectiveFunction import objectiveFunction as oF
from gauss import Matrix
from decimal import *

class ALS:
    def __init__(self, R: np.ndarray,d: int, reg: float):
        self.R = R
        self.d = d
        self.reg = reg
        self.numUsers = R.shape[0]
        self.numProducts = R.shape[1]
        self.U = self.createMatrix(d,self.numUsers)
        self.P = self.createMatrix(d,self.numProducts)
        
    def createMatrix(self, a: int, b: int) -> np.ndarray:
        return np.random.rand(a, b)
    
    def prediction(self):
        return np.matmul(self.U.T, self.P)
    
    def solveUser(self):
        for u in range(self.numUsers):
            I_u = np.flatnonzero(self.R[u])
            P_I_u = self.P[:, I_u]
            V_u = self.get_V_u(I_u, u)
            E = np.eye(self.d, dtype=np.float64)
            A_u = np.matmul(P_I_u, P_I_u.T) + self.reg * E
            
            matrix = Matrix(Decimal ,A_u, V_u)
            solution = matrix.gauss_part();
            self.U[:, u] = solution

    def solveProduct(self):
        for p in range(self.numProducts):
            I_p = np.flatnonzero(self.R[:, p])
            U_I_p = self.U[:, I_p]
            E = np.eye(self.d, dtype=np.float64)
            B_u = np.matmul(U_I_p, U_I_p.T) + self.reg * E
            W_p = self.get_W_p(I_p, p)
            
            matrix = Matrix(Decimal,B_u, W_p.T)
            solution = matrix.gauss_part();
            self.P[:, p] = solution


    def get_I_p(self, p) -> List[int]:
        I_p = list()
        for u_index, rate in enumerate(self.R[p]):
            if rate > 0:
                I_p.append(u_index)

        return I_p

    def get_W_p(self, I_p: List[int], p: int) -> np.ndarray:
        W_p = np.zeros(self.d, np.float64)
        for i in I_p:
            W_p += self.R[i, p] * self.U[:, i]

        return W_p

    def get_I_u(self, u: int) -> List[int]:
        I_u = list()
        for p_index, rate in enumerate(self.R[:, u]):
            if rate > 0:
                I_u.append(p_index)

        return I_u

    def get_V_u(self, I_u: List[int], u: int) -> np.ndarray:
        V_u = np.zeros(self.d).T
        for i in I_u:
            V_u += self.R[u, i] * self.P[:, i]

        return V_u

    def solve(self,  n_iters, ret_obj=True) -> list:
        obj_values = []
        for _ in range(n_iters):
            self.solveUser()
            self.solveProduct()
            if ret_obj:
                obj = oF(self.R, self.U, self.P, self.reg)
                obj_values.append(obj)

        return obj_values