import { DateTime } from 'luxon';

export default {
  quantity: {
    presence: true,
    numericality: {
      greaterThanOrEqual: 0
    }
  },
  basket: {

  },
  checkout: {

  },
  address: {
    presence: true
  },
  paymentMethod: {
    presence: true
  },
  addressForm: {
    alias: {
      presence: true,
      length: {
        minimum: 3,
        message: 'must be at least 3 characters'
      }
    },
    street: {
      presence: true,
      length: {
        minimum: 3,
        message: 'must be at least 3 characters'
      }
    },
    city: {
      presence: true,
      length: {
        minimum: 3,
        message: 'must be at least 3 characters'
      }
    },
    state: {
      presence: true,
    },
    zipCode: {
      presence: true,
      length: {
        is: 5,
        message: 'must be 5 characters'
      }
    },
    country: {
      presence: true,
    },
  },
  paymentMethodForm: {
    alias: {
      presence: true,
      length: {
        minimum: 3,
        message: 'must be at least 3 characters'
      }
    },
    cardholderName: {
      presence: true,
      length: {
        minimum: 3,
        message: 'must be at least 3 characters'
      }
    },
    cardNumber: {
      presence: true,
      credit_card: {}
    },
    expiration: {
      presence: true
    },
    securityNumber: {
      presence: true,
      length: {
        minimum: 3,
        maximum: 5,
        message: 'must be between 3 and 5 characters'
      }
    },
    cardType: {
      presence: true,
    },
  }
};
