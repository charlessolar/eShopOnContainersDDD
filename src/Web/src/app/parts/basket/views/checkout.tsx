import Avatar from '@material-ui/core/Avatar';
import Divider from '@material-ui/core/Divider';
import Grid from '@material-ui/core/Grid';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import Paper from '@material-ui/core/Paper';
import Step from '@material-ui/core/Step';
import StepLabel from '@material-ui/core/StepLabel';
import Stepper from '@material-ui/core/Stepper';
import Typography from '@material-ui/core/Typography';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import { observer } from 'mobx-react';
import * as React from 'react';
import { hot } from 'react-hot-loader';
import { Formatted, Using } from '../../../components/models';
import { sort } from '../../../utils';
import CheckoutAddress from '../components/checkoutAddress';
import CheckoutConfirm from '../components/checkoutConfirm';
import CheckoutPayment from '../components/checkoutPayment';
import { CheckoutStoreType } from '../stores/checkout';

interface CheckoutProps {
  store?: CheckoutStoreType;
}

const styles = (theme: Theme) => ({
  avatarQuantity: {
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.primary.contrastText,
    margin: theme.spacing.unit
  },
  avatar: {
    margin: 10,
    width: 60,
    height: 60
  },
  totals: {
    width: '100vw'
  }
});

@observer
class CheckoutView extends React.Component<CheckoutProps & WithStyles<'avatar' | 'avatarQuantity' | 'totals'>, { activeStep: number }> {
  constructor(props) {
    super(props);
    this.state = {
      activeStep: 0
    };
  }
  public componentDidMount() {
    const { store } = this.props;

    store.validateBasket();
  }

  private handleNext = () => {
    const { store } = this.props;
    const { activeStep } = this.state;

    if (activeStep === 2) {
      store.orderCompleted();
      return;
    }

    this.setState({activeStep: activeStep + 1});
  }
  private handlePrev = () => {
    const { activeStep } = this.state;
    this.setState({activeStep: activeStep - 1});
  }

  public render() {
    const { store, classes } = this.props;
    const { activeStep } = this.state;

    const items = sort(Array.from(store.items.values()), 'productId');
    return (
      <Grid container justify='center'>
        <Grid item xs={12} md={8}>
          <Grid container spacing={24}>
            <Grid item xs={12} md={8}>
              <Paper elevation={4}>
                <Stepper activeStep={activeStep}>
                  <Step completed={false}>
                    <StepLabel>Address</StepLabel>
                  </Step>
                  <Step completed={false}>
                    <StepLabel>Payment</StepLabel>
                  </Step>
                  <Step completed={false}>
                    <StepLabel>Confirm</StepLabel>
                  </Step>
                </Stepper>
                <div>
                  {activeStep === 0 && <CheckoutAddress checkout={store} handleNext={this.handleNext} handlePrev={this.handlePrev}/>}
                  {activeStep === 1 && <CheckoutPayment checkout={store} handleNext={this.handleNext} handlePrev={this.handlePrev}/>}
                  {activeStep === 2 && <CheckoutConfirm checkout={store} handleNext={this.handleNext} handlePrev={this.handlePrev}/>}
                </div>
              </Paper>
            </Grid>
            <Grid item xs={12} md={4}>
              <Paper elevation={4}>
                <Using model={store.basket}>
                  <List>
                    {items.map(item => (
                      <Using model={item} key={item.id}>
                        <ListItem>
                          <Avatar src={item.productPicture} className={classes.avatar} />
                          <ListItemText primary={item.productName} secondary={item.productDescription} />
                          <ListItemSecondaryAction>
                            <Avatar className={classes.avatarQuantity}>{item.quantity}</Avatar>
                          </ListItemSecondaryAction>
                        </ListItem>
                      </Using>
                    ))}
                    <Divider/>
                    <ListItem>
                      <Typography variant='title' color='primary' align='right' className={classes.totals}>SubTotal: <Formatted field='subTotal'/></Typography>
                    </ListItem>
                    <ListItem>
                      <Typography variant='subheading' color='primary' align='right' className={classes.totals}>Fees:</Typography>
                    </ListItem>
                    <ListItem>
                      <Typography variant='subheading' color='primary' align='right' className={classes.totals}>Taxes:</Typography>
                    </ListItem>
                    <ListItem>
                      <Typography variant='title' color='primary' align='right' className={classes.totals}>Total:</Typography>
                    </ListItem>
                  </List>
                </Using>
              </Paper>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
    );
  }
}

export default hot(module)(withStyles(styles as any)(CheckoutView));
