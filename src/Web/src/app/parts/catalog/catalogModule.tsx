import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { Brands, BrandsType, Brand, BrandType } from './models/brands';
import { Products, ProductsType, Product, ProductType } from './models/products';
import { Types, TypesType } from './models/types';
import { models } from '../../utils';

export class CatalogModule {
  public routes: UniversalRouterRoute[];

  constructor() {

    this.routes = [
      {
        path: '/brands',
        component: () => ({
          title: 'Brands',
          component: (
            <AsyncView
              actionStore={Brands}
              action={(brand: BrandsType) => brand.List.list()}
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
              actionStore={Types}
              action={(type: TypesType) => type.List.list()}
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
              actionStore={Products}
              action={(product: ProductsType) => product.List.list()}
              getComponent={() => import('./views/products')}
            />
          )
        })
      }
    ];
  }
}
