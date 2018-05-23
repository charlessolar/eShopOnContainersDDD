import { types, flow, getEnv, getParent, applySnapshot, getSnapshot } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import Debug from 'debug';

import { sort } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BuyerIndexType, BuyerIndexModel } from '../models/buyer';

const debug = new Debug('buyers');

export interface BuyerStoreType {
  loading: boolean;

  buyers: Map<string, BuyerIndexType>;

  get: () => Promise<{}>;
}

export const BuyerStoreModel = types
  .model({
    loading: types.optional(types.boolean, false),

    buyers: types.optional(types.map(BuyerIndexModel), {}),
  })
  .actions(self => {
    const get = flow(function*() {
      const request = new DTOs.Buyers();

      self.loading = true;
      try {
        const client = getEnv(self).api as ApiClientType;

        const results: DTOs.PagedResponse<DTOs.OrderingBuyerIndex> = yield client.paged(request);

        self.buyers.replace(results.records.map(x => [x.id, x]));
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });

    return { get };
  });
