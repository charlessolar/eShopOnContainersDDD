import Debug from 'debug';
import * as React from 'react';
import AsyncView from '../../components/asyncView';
import { StoreType } from '../../stores';
import { BasketStoreModel, BasketStoreType } from './stores/basket';
import { CheckoutStoreModel, CheckoutStoreType } from './stores/checkout';

const debug = new Debug('basket module');

export class BasketModule {
  public routes: UniversalRouterRoute[];

  constructor(store: StoreType) {

    this.routes = [
      {
        path: '/basket',
        component: () => ({
          title: 'Basket',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => BasketStoreModel.create({}, { api: store.api, history: store.history })}
              action={(store: BasketStoreType) => store.get()}
              getComponent={() => import('./views/basket')}
            />
          )
        })
      },
      {
        path: '/checkout',
        action: () => {
          debug('isAuth', store.authenticated);
          if (!store.authenticated) {
            return { redirect: '/login?nextPath=/checkout' };
          }
        },
        component: () => ({
          title: 'Checkout',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => CheckoutStoreModel.create({}, { api: store.api, history: store.history, alertStack: store.alertStack, auth: store.auth })}
              action={(store: CheckoutStoreType) => store.load()}
              loading={(store: CheckoutStoreType) => store.loading}
              getComponent={() => import('./views/checkout')}
            />
          )
        })
      }
    ];
  }
}
