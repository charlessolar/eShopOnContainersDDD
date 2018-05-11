import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { BrandsStoreModel, BrandsStoreType } from './stores/brands';
import { TypesStoreModel, TypesStoreType } from './stores/types';
import { ProductsStoreModel, ProductsStoreType } from './stores/products';
import { CatalogStoreModel, CatalogStoreType } from './stores/catalog';

export class CatalogModule {
  public routes: UniversalRouterRoute[];

  constructor() {

    this.routes = [
      {
        path: '/',
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
        path: '/brands',
        component: () => ({
          title: 'Brands',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => BrandsStoreModel.create({}, { api: store.api })}
              action={(store: BrandsStoreType) => store.get()}
              loading={(store: BrandsStoreType) => store.loading}
              getComponent={() => import('./views/brands')}
            />
          )
        })
      },
      {
        path: '/types',
        component: () => ({
          title: 'Type',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => TypesStoreModel.create({}, { api: store.api })}
              action={(store: TypesStoreType) => store.get()}
              loading={(store: TypesStoreType) => store.loading}
              getComponent={() => import('./views/types')}
            />
          )
        })
      },
      {
        path: '/products',
        component: () => ({
          title: 'Products',
          component: (
            <AsyncView
              actionStore={(store: StoreType) => ProductsStoreModel.create({}, { api: store.api })}
              action={(store: ProductsStoreType) => store.get()}
              loading={(store: ProductsStoreType) => store.loading}
              getComponent={() => import('./views/products')}
            />
          )
        })
      }
    ];
  }
}
