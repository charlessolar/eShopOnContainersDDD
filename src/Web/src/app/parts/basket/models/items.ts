import Debug from 'debug';
import { flow, getEnv, getParent, types } from 'mobx-state-tree';
import { FormatDefinition } from '../../../components/models';
import { ItemIndexModel as ItemIndexModelBase, ItemIndexType as ItemIndexTypeBase } from '../../../models/basket/items';
import { ApiClientType } from '../../../stores';
import { DTOs } from '../../../utils/eShop.dtos';
import { BasketType } from './basket';

const debug = new Debug('basket item');

export interface ItemIndexType extends ItemIndexTypeBase {
  loading: boolean;

  increaseQuantity: () => Promise<{}>;
  decreaseQuantity: () => Promise<{}>;
  readonly productPicture?: string;
  readonly formatting?: { [idx: string]: FormatDefinition };
}
export const ItemIndexModel = ItemIndexModelBase
  .props({
    loading: types.optional(types.boolean, false)
  })
  .actions(self => {
    const increaseQuantity = flow(function*() {
      debug('increasing quantity');

      const client = getEnv(self).api as ApiClientType;
      const basket = getParent(self, 2).basket as BasketType;

      try {
        const request = new DTOs.UpdateBasketItemQuantity();
        self.loading = true;

        request.basketId = self.basketId;
        request.productId = self.productId;
        request.quantity = self.quantity + 1;

        yield client.command(request);

        basket.subtractItem(self);

        self.quantity++;
        self.subTotal = self.productPrice * self.quantity;

        basket.additionItem(self);
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });
    const decreaseQuantity = flow(function*() {
      debug('decreasing quantity');

      const client = getEnv(self).api as ApiClientType;
      const basket = getParent(self, 2).basket as BasketType;

      try {
        self.loading = true;
        const request = new DTOs.UpdateBasketItemQuantity();

        request.basketId = self.basketId;
        request.productId = self.productId;
        request.quantity = self.quantity - 1;

        yield client.command(request);

        basket.subtractItem(self);

        self.quantity--;
        self.subTotal = self.productPrice * self.quantity;

        basket.additionItem(self);
      } catch (error) {
        debug('received http error: ', error);
        throw error;
      }
      self.loading = false;
    });

    return { increaseQuantity, decreaseQuantity };
  })
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
