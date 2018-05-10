import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import Debug from 'debug';

import { DTOs } from '../../../utils/eShop.dtos';
import { StoreType, ConfigurationStatusModel, ConfigurationStatusType } from '../../../stores';

const debug = new Debug('configuration setup');

export interface SetupType {
  loading: boolean;
  seed: () => Promise<{}>;
}
export const SetupModel = types
  .model({
    loading: types.optional(types.boolean, false)
  })
  .actions(self => {

    const seed = flow(function*() {
      const request = new DTOs.Seed();

      try {
        self.loading = true;
        const client = getEnv(self).store as StoreType;
        yield client.api.command(request);

        client.status.setup();
        setTimeout(() => client.history.push('/'), 1);
      } catch (error) {
        debug('received http error: ', error);
      }
      self.loading = false;
    });
    return { seed };
  });
