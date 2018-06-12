import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import { Theme, WithStyles, withStyles } from '@material-ui/core/styles';
import { observer } from 'mobx-react';
import * as React from 'react';
import { hot } from 'react-hot-loader';
import { sort } from '../../../utils';
import ProductFormAdd from '../components/catalog/ProductAdd';
import ProductRow from '../components/catalog/ProductRow';
import { CatalogStoreType } from '../stores/catalog';

interface CatalogProps {
  store?: CatalogStoreType;
}

const styles = (theme: Theme) => ({
  root: {
    marginTop: theme.spacing.unit * 3,
    width: '100%',
  },
  container: {
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto',
  },
  table: {
  },
});

@observer
class CatalogView extends React.Component<CatalogProps & WithStyles<'root' | 'container' | 'table' | 'avatar' | 'row' | 'button'>, {}> {

  public render() {
    const { store, classes } = this.props;

    const products = sort(Array.from(store.products.values()), 'id');
    return (
      <div className={classes.root}>
        <Grid container justify='center'>
          <Grid item xs={8}>
            <ProductFormAdd list={store} />
            <Paper className={classes.container}>
              <Table className={classes.table}>
                <TableHead>
                  <TableRow>
                    <TableCell>Product Name</TableCell>
                    <TableCell>Catagory Type</TableCell>
                    <TableCell>Catagory Brand</TableCell>
                    <TableCell>Price ($)</TableCell>
                    <TableCell>Available Stock</TableCell>
                    <TableCell>Thresholds</TableCell>

                  </TableRow>
                </TableHead>
                <TableBody>
                  {products.map(p => (
                    <ProductRow list={store} product={p} />
                  ))}
                </TableBody>
              </Table>
            </Paper>
          </Grid>
        </Grid>
      </div>
    );
  }
}

export default hot(module)(withStyles(styles as any)(CatalogView));
