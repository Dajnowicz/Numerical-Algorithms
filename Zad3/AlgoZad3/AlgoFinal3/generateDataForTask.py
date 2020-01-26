import json
import pandas as pd

from ALSAlgorithm import ALS
from processData import sort, create_data, get_time_and_result
from objectiveFunction import objectiveFunction as oF

def generate_data_for_first_task(df,numUsers, numProducts, fileName, category='Music'):
    num_of_iterations = 15
    d_values = [3, 5 ,8 ,13 ,21]
    reg_values = [0.01, 0.1, 0.5, 1]

    VALS = []

    rates = sort(df=df, category='Music', users=numUsers, products=numProducts, sort_user=True)
    import csv

    for d in d_values:
        for reg in reg_values:
            als = ALS(R=rates, d=d, reg=reg)
            obj_values = als.solve(num_of_iterations)
            VALS = obj_values
            #print(VALS)
            with open(fileName, "a") as fp:
                wr = csv.writer(fp, lineterminator='\n')
                for val in VALS:
                    wr.writerow([val])


def generate_data_for_second_task(df, users, products, category='Music'):
    num_of_iterations = 100
    d_values = [2, 3,5 ,8 ,13 ,21, 25, 28]
    reg = 0.1
    VALS = []
    rates = sort(df, category=category, users=users, products=products, sort_user=True)
    train, test = create_data(rates, 20)
    
    for d in d_values:
        als = ALS(R=rates, d=d, reg=reg)
        elapsed_time, _ = get_time_and_result(als.solve, num_of_iterations, False)
        obj_values = oF(test, als.U, als.P, reg)
        VALS.append(dict(d=d, reg=reg, obj_values=obj_values,time=elapsed_time))

    return VALS

