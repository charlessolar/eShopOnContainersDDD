import * as React from 'react';
import AsyncView from '../../components/asyncView';
import { StoreType } from '../../stores';
import ToolbarView from './components/administrateToolbar';
import { BuyerStoreModel, BuyerStoreType } from './stores/buyers';
import { CatalogStoreModel, CatalogStoreType } from './stores/catalog';
import { DashboardStoreModel, DashboardStoreType } from './stores/dashboard';
import { OrdersStoreModel, OrdersStoreType } from './stores/orders';

export class AdministrationModule {
  public routes: UniversalRouterRoute[];

  constructor(store: StoreType) {

    this.routes = [
      {
        path: '/administrate',
        action: (ctx) => {
          if (!store.auth.admin) {
            store.alertStack.add('error', 'not an admin');
            return { redirect: '/' };
          }
          return ctx.next().then(child => ({ title: 'Administrate ' + (child as any).title, component: (<ToolbarView>{child.component}</ToolbarView>)}));
        },
        children: [
          {
            path: '',
            component: () => ({
              title: 'Dashboard',
              component: (
                <AsyncView
                  actionStore={(store: StoreType) => DashboardStoreModel.create({}, { api: store.api })}
                  action={(store: DashboardStoreType) => store.get()}
                  loading={(store: DashboardStoreType) => store.loading}
                  getComponent={() => import('./views/dashboard')}
                />
              )
            })
          },
          {
            path: '/catalog',
            component: () => ({
              title: 'Catalog',
              component: (
                <AsyncView
                  actionStore={(store: StoreType) => CatalogStoreModel.create({}, { api: store.api })}
                  action={(store: CatalogStoreType) => store.get()}
                  loading={(store: CatalogStoreType) => store.loading}
                  getComponent={() => import('./views/catalog')}
                />
              )
            })
          },
          {
            path: '/orders',
            component: () => ({
              title: 'Orders',
              component: (
                <AsyncView
                  actionStore={(store: StoreType) => OrdersStoreModel.create({}, { api: store.api })}
                  action={(store: OrdersStoreType) => store.get()}
                  loading={(store: OrdersStoreType) => store.loading}
                  getComponent={() => import('./views/orders')}
                />
              )
            })
          },
          {
            path: '/buyers',
            component: () => ({
              title: 'Buyers',
              component: (
                <AsyncView
                  actionStore={(store: StoreType) => BuyerStoreModel.create({}, { api: store.api })}
                  action={(store: BuyerStoreType) => store.get()}
                  loading={(store: BuyerStoreType) => store.loading}
                  getComponent={() => import('./views/buyers')}
                />
              )
            })
          }
        ]
      }
    ];
  }
}
