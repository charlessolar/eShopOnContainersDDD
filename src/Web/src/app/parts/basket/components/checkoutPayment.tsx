import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Typography from 'material-ui/Typography';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';

import { inject } from '../../../utils';
import { Using, Formatted, Field } from '../../../components/models';

import { PaymentMethodStoreType, PaymentMethodStoreModel } from '../stores/paymentMethod';
import { CheckoutStoreType } from '../stores/checkout';

interface PaymentMethodProps {
  handleNext: () => void;
  handlePrev: () => void;

  store?: PaymentMethodStoreType;
  checkout: CheckoutStoreType;
}

const styles = (theme: Theme) => ({

});

@inject(PaymentMethodStoreModel)
@observer
class PaymentMethodView extends React.Component<PaymentMethodProps & WithStyles<never>, {}> {

  public render() {
    const { handleNext, handlePrev } = this.props;

    return (
      <div>
        <Button variant='raised' onClick={handlePrev}>Prev</Button>
        <Button variant='raised' onClick={handleNext}>Next</Button>
      </div>
    );
  }
}

export default withStyles(styles)<PaymentMethodProps>(PaymentMethodView);
