import * as React from 'react';
import { types, getRoot } from 'mobx-state-tree';

import AsyncView from '../../components/asyncView';
import { models } from '../../utils';
import { StoreType } from '../../stores';

import { SetupType, SetupModel } from './stores/setup';
import SetupView from './views/setup';

export class ConfigurationModule {
  public routes: UniversalRouterRoute[];

  constructor(store: StoreType) {

    this.routes = [
      {
        path: '/seed',
        action: () => {
          if (store.status.isSetup) {
            setTimeout(() => store.history.push('/'), 1);
            throw new Error('redirect');
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
