import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';
import glamorous from 'glamorous';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import TableFooter from '@material-ui/core/TableFooter';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Avatar from '@material-ui/core/Avatar';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import Button from '@material-ui/core/Button';
import IconButton from '@material-ui/core/IconButton';
import Tooltip from '@material-ui/core/Tooltip';
import AddIcon from '@material-ui/icons/Add';
import DeleteIcon from '@material-ui/icons/Delete';
import RemoveIcon from '@material-ui/icons/Remove';

import { sort } from '../../../utils';

import { Using, Formatted, Field } from '../../../components/models';
import { BasketStoreType, BasketStoreModel } from '../stores/basket';

interface BasketProps {
  store?: BasketStoreType;
}

const styles = (theme: Theme) => ({
  appbar: {
    boxShadow: '0 4px 2px -2px gray',
    backgroundColor: theme.palette.grey[100]
  },
  none: {
    height: '80vh',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  table: {
  },
  flex: {
    flex: 1
  },
  container: {
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  avatar: {
    margin: 10,
    width: 60,
    height: 60
  },
  row: {
    '&:nth-of-type(odd)': {
      backgroundColor: theme.palette.background.default,
    },
  },
  button: {
    margin: theme.spacing.unit,
    color: theme.palette.primary.light
  },
  quantityButton: {
    width: 24,
    height: 24,
    color: theme.palette.primary.light,
    margin: theme.spacing.unit,
  },
  total: {
    color: theme.palette.error[900]
  }
});

const AppView = glamorous('div')((_) => ({
  flex: '1 1 auto',
  width: '100vw',
  display: 'flex',
  flexDirection: 'column',
  overflow: 'auto',
  justifyContent: 'center',
  alignItems: 'center',
}));
const MainView = glamorous('main')({
  'width': '75vw',
  'flex': '1',
  'marginTop': 20,
  '@media(max-width: 600px)': {
    margin: 10
  }
});

@observer
class BasketView extends React.Component<BasketProps & WithStyles<'none' | 'appbar' | 'flex' | 'container' | 'table' | 'avatar' | 'row' | 'button' | 'total' | 'quantityButton'>, {}> {

  private checkout = () => {
    const { store } = this.props;

    store.checkout();
  }

  public render() {
    const { store, classes } = this.props;

    const items = sort(Array.from(store.items.values()), 'productId');
    return (
      <Using model={store.basket}>
        <AppView>
          <AppBar position='static' className={classes.appbar}>
            <Toolbar>
              <div className={classes.flex}>
                <Typography variant='headline'>Shopping Basket</Typography>
              </div>
              <Button variant='raised' color='primary' disabled={!store.basket || items.length === 0} onClick={this.checkout}>Checkout</Button>
            </Toolbar>
          </AppBar>
          <MainView>
            {!store.basket || items.length === 0 ?

              <Grid container justify='center'>
                <Grid item xs={6}>
                  <Typography variant='display3' className={classes.none}>Shopping basket is empty!</Typography>
                </Grid>
              </Grid>
              :
              <Grid container justify='center'>
                <Grid item xs={8}>
                  <Paper className={classes.container}>
                    <Table className={classes.table}>
                      <TableHead>
                        <TableRow>
                          <TableCell>Product</TableCell>
                          <TableCell>Price ($)</TableCell>
                          <TableCell>Quantity</TableCell>
                          <TableCell>SubTotal ($)</TableCell>
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {items.map(i => (
                          <TableRow hover key={i.id} className={classes.row}>
                            <Using model={i}>
                              <TableCell component='th' scope='row'>
                                <ListItem>
                                  <ListItemIcon>
                                    <Tooltip title='Delete Item'>
                                      <IconButton className={classes.button} color='primary' area-label='Delete Item' disabled={i.loading} onClick={() => store.removeItem(i)}>
                                        <DeleteIcon />
                                      </IconButton>
                                    </Tooltip>
                                  </ListItemIcon>
                                  <Avatar src={i.productPicture} className={classes.avatar} />
                                  <ListItemText primary={i.productName} secondary={i.productDescription} />
                                </ListItem>
                              </TableCell>
                              <TableCell numeric>
                                <Typography variant='title' color='primary'>
                                  <Formatted field='productPrice' />
                                </Typography>
                              </TableCell>
                              <TableCell numeric>
                                <IconButton className={classes.quantityButton} color='primary' area-label='Decrease Quantity' disabled={i.loading || i.quantity === 1} onClick={() => i.decreaseQuantity()}>
                                  <RemoveIcon />
                                </IconButton>
                                <Formatted field='quantity' />
                                <IconButton className={classes.quantityButton} color='primary' area-label='Increase Quantity' disabled={i.loading} onClick={() => i.increaseQuantity()}>
                                  <AddIcon />
                                </IconButton>
                              </TableCell>
                              <TableCell numeric>
                                <Typography variant='title' color='primary'>
                                  <Formatted field='subTotal' />
                                </Typography>
                              </TableCell>
                            </Using>
                          </TableRow>
                        ))}
                      </TableBody>
                      <TableFooter>
                        <TableRow>
                          <TableCell colSpan={2}></TableCell>
                          <TableCell component='th'>
                            <Typography variant='headline' color='primary'>
                              SubTotal
                            </Typography>
                          </TableCell>
                          <TableCell numeric>
                            <Typography variant='headline' color='primary'>
                              <Formatted field='subTotal' />
                            </Typography>
                          </TableCell>
                        </TableRow>
                      </TableFooter>
                    </Table>
                  </Paper>
                </Grid>
              </Grid>

            }
          </MainView>
        </AppView>
      </Using>
    );
  }
}

export default hot(module)(withStyles(styles as any)(BasketView));
