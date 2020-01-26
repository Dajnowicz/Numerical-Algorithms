import re
from timeit import default_timer as timer
import numpy as np
import pandas as pd

def sort(df= pd.DataFrame, category = 'Books', users= 100, products = 10, sort_user = False):
    #wybranie jednej kategorii
    one_cat = df[df.category == category]
    #wybranie produktow ktore maja najwiecej reviews
    naj_prod = one_cat[one_cat.product_id.isin(one_cat.groupby(by='product_id').count().sort_values('review', ascending=False).index[:products].tolist())]
    
    #znalezenie osob ktore daly najwiecej revsow
    ids_most_active_users_of_category = naj_prod.groupby('customer_id').count()
    
    #posortowanie ludzi z najwieksza iloscia revsow
    if sort_user:
        ids_most_active_users_of_category = ids_most_active_users_of_category.sort_values('review', ascending=False)
    
    #pivot na tabelce
    pivot_table = pd.pivot_table(naj_prod[naj_prod.customer_id.isin(ids_most_active_users_of_category.index[:users].tolist())], values=['review'], columns=['product_id'], index=['customer_id'], aggfunc='first', fill_value=0)
    rates = pivot_table['review'].values
    return rates


def create_data(ratings, testSize):
    test = np.zeros(ratings.shape)
    train = ratings.copy()
    for user in range(ratings.shape[0]):
        ratedProducts = np.flatnonzero(ratings[user])
        testSize = int(np.round(len(ratedProducts) * (testSize / 100))) or 1
        testIndexes = np.random.choice(
            ratedProducts, size=testSize)

        train[user, testIndexes] = 0.0
        test[user, testIndexes] = ratings[user, testIndexes]

    return train, test
        

def get_time_and_result(func, *fargs, **fkwargs):
    start = timer()
    f_result = func(*fargs, **fkwargs)
    end = timer()
    elapsed_time = end - start
    return elapsed_time, f_result