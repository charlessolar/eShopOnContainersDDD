import * as React from 'react';
import { StoreType } from '../../stores';
import SetupView from './views/setup';

export class ConfigurationModule {
  public routes: UniversalRouterRoute[];

  constructor(store: StoreType) {

    this.routes = [
      {
        path: '/seed',
        action: () => {
          if (store.status.isSetup) {
            return { redirect: '/' };
          }
        },
        component: () => ({
          title: 'Seed',
          component: (
            <SetupView/>
          )
        })
      }
    ];
  }
}
