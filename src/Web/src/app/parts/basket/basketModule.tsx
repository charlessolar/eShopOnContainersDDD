import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { BasketStoreModel, BasketStoreType } from './stores/basket';
import { CheckoutStoreModel, CheckoutStoreType } from './stores/checkout';

export class BasketModule {
  public routes: UniversalRouterRoute[];

  constructor() {

    this.routes = [
      {
        path: '/basket',
        component: () => ({
          title: 'Basket',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => BasketStoreModel.create({}, { api: store.api })}
              action={(store: BasketStoreType) => store.get()}
              loading={(store: BasketStoreType) => store.loading}
              getComponent={() => import('./views/basket')}
            />
          )
        })
      },
      {
        path: '/checkout',
        component: () => ({
          title: 'Checkout',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => CheckoutStoreModel.create({}, { api: store.api })}
              action={(store: CheckoutStoreType) => store.get()}
              loading={(store: CheckoutStoreType) => store.loading}
              getComponent={() => import('./views/checkout')}
            />
          )
        })
      }
    ];
  }
}
