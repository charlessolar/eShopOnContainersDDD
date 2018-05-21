import * as React from 'react';
import { observer } from 'mobx-react';
import { getSnapshot } from 'mobx-state-tree';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Button from '@material-ui/core/Button';
import Divider from '@material-ui/core/Divider';

import { inject } from '../../../utils';
import { Using, Formatted, Field, Submit } from '../../../components/models';

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
    marginLeft: 20,
    marginBottom: 20,
    alignSelf: 'flex-start'
  },
  divider: {
    marginTop: 20,
  }
});

@inject(PaymentMethodStoreModel, 'store', 'checkout', (s: CheckoutStoreType) => {
  return {
    paymentMethod: s.selectedPaymentMethod || s.buyer.preferredPaymentMethodId,
  };
})
@observer
class PaymentMethodView extends React.Component<PaymentMethodProps & WithStyles<'paper' | 'stepActions' | 'stepButton' | 'divider'>, {}> {

  private handleNext = () => {
    const { store, checkout, handleNext } = this.props;

    checkout.selectPayment(getSnapshot(store.paymentMethod));

    handleNext();
  }
  public render() {
    const { classes, store, handleNext, handlePrev } = this.props;

    return (
      <Using model={store}>
        <Grid container justify='center'>
          <Grid item md={6} xs={12}>
            <Paper className={classes.paper} elevation={2}>
              <Typography variant='headline' gutterBottom color='primary'>Payment Method</Typography>
              {store.paymentMethod ? (
                <div>
                  <Typography variant='title'>{store.paymentMethod.cardholderName}</Typography>
                  <Typography component='p'>
                    {store.paymentMethod.cardType}<br />
                    {store.paymentMethod.cardNumber} <strong>{store.paymentMethod.expirationMonthYear}</strong><br />
                    {store.paymentMethod.securityNumber}
                  </Typography>
                </div>
              ) : (<Typography variant='title'>( none )</Typography>)}
            </Paper>
          </Grid>
          <Grid item md={4} xs={12}>
            <Field field='paymentMethod' />
          </Grid>
        </Grid>

        <Divider className={classes.divider} />
        <div className={classes.stepActions}>
          <Button variant='raised' onClick={handlePrev} className={classes.stepButton}>Prev</Button>
          <Submit buttonProps={{ variant: 'raised', className: classes.stepButton, color: 'primary' }} onSuccess={this.handleNext} text='Next' />
        </div>
      </Using>
    );
  }
}

export default withStyles(styles)<PaymentMethodProps>(PaymentMethodView);
