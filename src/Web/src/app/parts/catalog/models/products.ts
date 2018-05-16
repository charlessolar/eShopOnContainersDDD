import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

import rules from '../validation';
import { models } from '../../../utils';
import { FormatDefinition, FieldDefinition } from '../../../components/models';

import { DTOs } from '../../../utils/eShop.dtos';
import { ApiClientType } from '../../../stores';

import { TypeModel, TypeType, TypeListModel, TypeListType } from './types';
import { BrandModel, BrandType, BrandListModel, BrandListType } from './brands';

import { ProductType as ProductTypeBase, ProductModel as ProductModelBase } from '../../../models/catalog/products';

const debug = new Debug('catalog products');

export interface ProductType extends ProductTypeBase {
  readonly formatting?: {[idx: string]: FormatDefinition };
  readonly productPicture?: string;
  readonly canOrder?: boolean;
}
export const ProductModel =
  ProductModelBase
  .views(self => ({
    get formatting() {
      return ({
        price: {
          currency: true,
          normalize: 2
        }
      });
    },
    get productPicture() {
      if (!self.pictureContents) {
        return;
      }
      return 'data:' + self.pictureContentType + ';base64,' + self.pictureContents;
    },
    get canOrder() {
      return self.availableStock > 0 || self.onReorder;
    }
  }));
