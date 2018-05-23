import { DateTime } from 'luxon';
import { types } from 'mobx-state-tree';

export interface PaymentMethodType {
  id: string;

  alias: string;
  cardNumber: string;
  securityNumber: string;
  cardholderName: string;
  expiration: string;
  cardType: string;

  expirationMonthYear: string;
}
export const PaymentMethodModel = types
  .model('Ordering_Buyer_PaymentMethod', {
    id: types.identifier(types.string),

    alias: types.string,
    cardNumber: types.string,
    securityNumber: types.string,
    cardholderName: types.string,
    expiration: types.string,
    cardType: types.union(types.literal('VISA'), types.literal('AMEX'), types.literal('MASTERCARD'))
  })
  .views(self => ({
    get expirationMonthYear() {
      return DateTime.fromISO(self.expiration).toFormat('MM/yy');
    }
  }));
