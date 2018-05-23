import Divider from '@material-ui/core/Divider';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import CreditIcon from '@material-ui/icons/CreditCard';
import MailIcon from '@material-ui/icons/Mail';
import ReceiptIcon from '@material-ui/icons/Receipt';
import * as React from 'react';
import { Submit, Using } from '../../../components/models';
import { inject } from '../../../utils';
import { CheckoutStoreType } from '../stores/checkout';
import { ConfirmStoreModel, ConfirmStoreType } from '../stores/confirm';

interface ConfirmProps {
  handleNext: () => void;
  handlePrev: () => void;

  store?: ConfirmStoreType;
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
    marginTop: 20
  },
  iconContainer: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center'
  },
  icons: {
    fontSize: 80,
  }
});

@inject(ConfirmStoreModel, 'store', 'checkout', (s: CheckoutStoreType) => {
  return {
    basketId: s.basket.id,
    billingAddress: s.selectedBillingAddress,
    shippingAddress: s.selectedShippingAddress,
    paymentMethod: s.selectedPaymentMethod
  };
})
class ConfirmView extends React.Component<ConfirmProps & WithStyles<'paper' | 'stepActions' | 'stepButton' | 'divider' | 'iconContainer' | 'icons'>, {}> {

  private handleSuccess = () => {
    const { handleNext } = this.props;
    handleNext();
  }

  public render() {
    const { store, classes, handleNext, handlePrev } = this.props;

    return (
      <Using model={store}>

        <Grid container justify='center'>
          <Grid item md={8} xs={12}>
            <Paper elevation={2} className={classes.paper}>
              <Grid container>
                <Grid item xs={8}>
                  <Typography variant='headline' gutterBottom color='primary'>Billing Address</Typography>
                  <Typography variant='title'>{store.billingAddress.street}</Typography>
                  <Typography component='p'>
                    {store.billingAddress.city}, {store.billingAddress.state} {store.billingAddress.zipCode}<br />
                    {store.billingAddress.country}
                  </Typography>
                </Grid>
                <Grid item xs={4} className={classes.iconContainer}>
                  <ReceiptIcon className={classes.icons} />
                </Grid>
              </Grid>
            </Paper>
            <Paper elevation={2} className={classes.paper}>
              <Grid container>
                <Grid item xs={8}>
                  <Typography variant='headline' gutterBottom color='primary'>Shipping Address</Typography>
                  <Typography variant='title'>{store.shippingAddress.street}</Typography>
                  <Typography component='p'>
                    {store.shippingAddress.city}, {store.shippingAddress.state} {store.shippingAddress.zipCode}<br />
                    {store.shippingAddress.country}
                  </Typography>
                  </Grid>
                <Grid item xs={4} className={classes.iconContainer}>
                  <MailIcon className={classes.icons} />
                </Grid>
              </Grid>
            </Paper>
            <Paper elevation={2} className={classes.paper}>
              <Grid container>
                <Grid item xs={8}>
                  <Typography variant='headline' gutterBottom color='primary'>Payment Method</Typography>
                  <Typography variant='title'>{store.paymentMethod.cardholderName}</Typography>
                  <Typography component='p'>
                    {store.paymentMethod.cardType}<br />
                    {store.paymentMethod.cardNumber} <strong>{store.paymentMethod.expirationMonthYear}</strong><br />
                    {store.paymentMethod.securityNumber}
                  </Typography>
                </Grid>
                <Grid item xs={4} className={classes.iconContainer}>
                  <CreditIcon className={classes.icons} />
                </Grid>
              </Grid>
            </Paper>
          </Grid>
        </Grid>

        <Divider className={classes.divider} />
        <div className={classes.stepActions}>
          <Submit buttonProps={{ variant: 'raised', className: classes.stepButton, color: 'primary' }} onSuccess={this.handleSuccess} text='Complete!' />
        </div>
      </Using>
    );
  }
}

export default withStyles(styles)<ConfirmProps>(ConfirmView);
