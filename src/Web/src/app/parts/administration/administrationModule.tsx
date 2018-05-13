import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { CatalogStoreModel, CatalogStoreType } from './stores/catalog';

export class AdministrationModule {
  public routes: UniversalRouterRoute[];

  constructor() {

    this.routes = [
      {
        path: '/administrate',
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
      }
    ];
  }
}
