import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { OrderStoreType, OrderStoreModel } from './stores/orders';

export class OrdersModule {
  public routes: UniversalRouterRoute[];

  constructor(store: StoreType) {

    this.routes = [
      {
        path: '/orders',
        action: () => {
          if (!store.authenticated) {
            store.alertStack.add('error', 'not logged in');
            return { redirect: '/' };
          }
        },
        component: () => ({
          title: 'Orders',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => OrderStoreModel.create({}, { api: store.api })}
              action={(store: OrderStoreType) => store.get()}
              getComponent={() => import('./views/orders')}
            />
          )
        })
      }
    ];
  }
}
