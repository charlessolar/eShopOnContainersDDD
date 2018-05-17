import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BasketType as BasketTypeBase, BasketModel as BasketModelBase } from '../../../models/basket/baskets';

const debug = new Debug('basket');

export interface BasketType extends BasketTypeBase {
  readonly createdDate?: DateTime;
  readonly updatedDate?: DateTime;
  readonly formatting?: { [idx: string]: FormatDefinition };
}
export const BasketModel = BasketModelBase
  .views(self => ({
    get createdDate() {
      return DateTime.fromMillis(self.created);
    },
    get updatedDate() {
      return DateTime.fromMillis(self.updated);
    },
    get formatting() {
      return ({
        subTotal: {
          currency: true,
          normalize: 2
        },
        totalFees: {
          currency: true,
          normalize: 2
        },
        totalTaxes: {
          currency: true,
          normalize: 2
        },
        total: {
          currency: true,
          normalize: 2
        }
      });
    },
  }));
