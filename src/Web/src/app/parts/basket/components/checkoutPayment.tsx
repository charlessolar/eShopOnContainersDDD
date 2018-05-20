import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';

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
  paper: {
    minHeight: 120,
    padding: 20
  },
  stepActions: {
    margin: 20,
    display: 'flex',
    justifyContent: 'flex-end',
  },
  stepButton: {
    alignSelf: 'flex-start'
  }
});

@inject(PaymentMethodStoreModel, 'store', 'checkout', (s: CheckoutStoreType) => {
  return {
    paymentMethod: s.buyer.preferredPaymentMethodId,
  };
})
@observer
class PaymentMethodView extends React.Component<PaymentMethodProps & WithStyles<'paper' | 'stepActions' | 'stepButton'>, {}> {

  public render() {
    const { classes, store, handleNext, handlePrev } = this.props;

    return (
      <Using model={store}>
      <Grid container justify='center'>
      <Grid item xs={6}>
            <Paper className={classes.paper} elevation={2}>
            <Typography variant='headline' gutterBottom color='primary'>Payment Method</Typography>
              {store.paymentMethod ? (
                <div>
                  <Typography variant='title'>{store.paymentMethod.cardholderName}</Typography>
                  <Typography component='p'>
                    {store.paymentMethod.cardType}<br/>
                    {store.paymentMethod.cardNumber} {store.paymentMethod.expiration}<br />
                    {store.paymentMethod.securityNumber}
                  </Typography>
                </div>
              ) : (<Typography variant='title'>( none )</Typography>)}
              </Paper>
          </Grid>
          <Grid item xs={4}>
            <Field field='paymentMethod' />
          </Grid>
        </Grid>

        <div className={classes.stepActions}>
          <Button variant='raised' onClick={handlePrev} className={classes.stepButton}>Prev</Button>
          <Button variant='raised' onClick={handleNext} className={classes.stepButton} color='primary'>Next</Button>
        </div>
      </Using>
    );
  }
}

export default withStyles(styles)<PaymentMethodProps>(PaymentMethodView);
