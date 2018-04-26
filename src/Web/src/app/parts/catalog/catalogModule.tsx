import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { Brands, BrandsType, Brand, BrandType } from './models/brands';
import { Types, TypesType } from './models/types';
import { models } from '../../utils';

export interface CatalogStoresType {
  Brands: BrandsType;
}
export const CatalogStores = types.model(
  'Catalog',
  {
    Brands: types.optional(Brands, {}),
    Types: types.optional(Types, {})
  }
);

export class CatalogModule {
  public routes: UniversalRouterRoute[];

  constructor(private _store: CatalogStoresType) {

    this.routes = [
      {
        path: '/',
        component: () => ({
          title: 'Home',
          component: (
            <AsyncView
              action={this._store.Brands.List.list}
              getComponent={() => import('./views/brands')}
            />
          )
        })
      }
    ];
  }
}
