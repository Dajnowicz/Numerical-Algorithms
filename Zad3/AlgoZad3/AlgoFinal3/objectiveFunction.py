import numpy as np

def objectiveFunction(R: np.ndarray, U: np.ndarray, P: np.ndarray, factor: float) -> float:
    result = 0
    regularization = 0

    for u in range(U.shape[1]):
        for p in range(P.shape[1]):
            if R[u, p] != 0:
                result += np.power(R[u, p] - np.matmul(U[:, u].T, P[:, p]), 2)

    for u in range(U.shape[1]):
        regularization += np.power(np.linalg.norm(U[:, u]), 2)

    for p in range(P.shape[1]):
        regularization += np.power(np.linalg.norm(P[:, p]), 2)

    result += factor * regularization

    return result