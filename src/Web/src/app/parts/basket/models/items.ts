import { types, getRoot, getEnv, flow, getParent } from 'mobx-state-tree';
import * as validate from 'validate.js';
import { DateTime } from 'luxon';
import uuid from 'uuid/v4';
import Debug from 'debug';

import { models } from '../../../utils';
import { FormatDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { BasketType } from './basket';
import { ItemIndexType as ItemIndexTypeBase, ItemIndexModel as ItemIndexModelBase } from '../../../models/basket/items';

const debug = new Debug('basket item');

export interface ItemIndexType extends ItemIndexTypeBase {
  updateQuantity: (quantity: number) => void;
  readonly productPicture?: string;
  readonly formatting?: { [idx: string]: FormatDefinition };
}
export const ItemIndexModel = ItemIndexModelBase
  .actions(self => ({
    updateQuantity(quantity: number) {
      const basket = getParent(self, 2) as BasketType;
      basket.subTotal -= self.subTotal;
      basket.totalQuantity -= self.quantity;

      self.quantity = quantity;
      self.subTotal = self.productPrice * self.quantity;

      basket.subTotal += self.subTotal;
      basket.totalQuantity += self.quantity;
    }
  }))
  .views(self => ({
    get productPicture() {
      if (!self.productPictureContents) {
        return;
      }
      return 'data:' + self.productPictureContentType + ';base64,' + self.productPictureContents;
    },
    get formatting() {
      return ({
        productPrice: {
          currency: true,
          normalize: 2
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
        },
        quantity: {
          trim: true
        }
      });
    },
  }));
