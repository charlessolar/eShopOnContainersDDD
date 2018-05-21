import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FormatDefinition, FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { OrderType as OrderTypeBase, OrderModel as OrderModelBase } from '../../../models/ordering/order';
import { OrderItemType, OrderItemModel } from '../models/item';

const debug = new Debug('orders');

export interface OrderType extends OrderTypeBase {
  items: OrderItemType[];

  readonly placed: string;
  readonly formatting: {[idx: string]: FieldDefinition};
}
export const OrderModel = OrderModelBase
  .props({
    items: types.array(OrderItemModel)
  })
  .views(self => ({
    get formatting() {
      return ({
        totalItems: {
          trim: true,
        },
        totalQuantity: {
          trim: true
        },
        subTotal: {
          currency: true,
          normalize: 2
        },
        additional: {
          currency: true,
          normalize: 2
        },
        total: {
          currency: true,
          normalize: 2
        }
      });
    },
    get placed() {
      return DateTime.fromMillis(self.created).toFormat('DDD');
    }
  }));
