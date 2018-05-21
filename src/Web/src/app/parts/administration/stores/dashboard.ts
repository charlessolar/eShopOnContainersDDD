import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import Debug from 'debug';

import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

const debug = new Debug('dashboard');

export interface DashboardStoreType {
  loading: boolean;
  get: () => Promise<{}>;
}

export const DashboardStoreModel = types
  .model('DashboardStore', {
    loading: types.optional(types.boolean, false),
  })
  .actions(self => {
    const get = flow(function*() {
      return;
    });

    return {get};
  });
