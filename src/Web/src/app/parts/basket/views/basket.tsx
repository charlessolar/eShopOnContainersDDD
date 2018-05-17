import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';
import glamorous from 'glamorous';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Table, { TableBody, TableCell, TableHead, TableRow, TableFooter } from 'material-ui/Table';
import { ListItem, ListItemText, ListItemIcon } from 'material-ui/List';
import Typography from 'material-ui/Typography';
import Grid from 'material-ui/Grid';
import Paper from 'material-ui/Paper';
import Avatar from 'material-ui/Avatar';
import AppBar from 'material-ui/AppBar';
import Toolbar from 'material-ui/Toolbar';
import Button from 'material-ui/Button';
import IconButton from 'material-ui/IconButton';
import Tooltip from 'material-ui/Tooltip';
import DeleteIcon from '@material-ui/icons/delete';

import { Using, Formatted, Field } from '../../../components/models';
import { BasketStoreType, BasketStoreModel } from '../stores/basket';

import QuantityForm from '../components/quantityForm';

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
class BasketView extends React.Component<BasketProps & WithStyles<'none' | 'appbar' | 'flex' | 'container' | 'table' | 'avatar' | 'row' | 'button' | 'total'>, {}> {

  public render() {
    const { store, classes } = this.props;

    const items = Array.from(store.items.values());
    return (
      <Using model={store.basket}>
        <AppView>
          <AppBar position='static' className={classes.appbar}>
            <Toolbar>
              <div className={classes.flex}>
                <Typography variant='headline'>Shopping Basket</Typography>
              </div>
              <Button variant='raised' color='primary'>Checkout</Button>
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
                                      <IconButton className={classes.button} color='primary' area-label='Delete Item' onClick={() => store.removeItem(i)}>
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
                                <Formatted field='quantity' />
                                <QuantityForm item={i}/>
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
