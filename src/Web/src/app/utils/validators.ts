import * as validate from 'validate.js';
import { DateTime } from 'luxon';

import { Data } from './image';

export default function install() {
  validate.validators.json = (value) => {
    if (!value) {
      return 'invalid json object';
    }
    try {
      JSON.parse(value);
    } catch {
      return 'invalid json object';
    }
  };

  validate.validators.image = (value: Data, options: { ratio?: number }) => {
    if (!value) {
      return;
    }

    if (options && options.ratio && (Math.abs(options.ratio - (value.width / value.height)) > 0.01)) {
      return 'incorrect image ratio';
    }
  };

  validate.validators.moment = (value, options) => {
    if (!value) {
      return 'invalid date value';
    }
    if (!DateTime.fromFormat(value, options && options.format ? options.format : 'D').isValid) {
      return 'invalid date format';
    }
  };

  validate.validators.array_inclusion = (value: any[], options) => {
    if (!validate.isArray(value)) {
      return 'is not an array';
    }
    const valids = value.map(v => validate.validators.inclusion(v, options));
    if (valids.filter(v => v).length) {
      return 'invalid array elements';
    }
  };
  validate.validators.credit_card = (value: string, options) => {
    if (!value) {
      return;
    }

    if (/[^0-9-\s]+/.test(value)) {
      return 'invalid number';
    }

    // The Luhn Algorithm. It's so pretty.
    let nCheck = 0;
    let nDigit = 0;
    let bEven = false;
    value = value.replace(/\D/g, '');

    for (let n = value.length - 1; n >= 0; n--) {
      const cDigit = value.charAt(n);
      nDigit = parseInt(cDigit, 10);

      if (bEven) {
        nDigit *= 2;
        if (nDigit > 9) {
          nDigit -= 9;
        }
      }

      nCheck += nDigit;
      bEven = !bEven;
    }

    if ((nCheck % 10) !== 0) {
      return 'invalid card number';
    }
  };
}
