import * as React from 'react';
import { observable, action, computed } from 'mobx';
import { observer, inject } from 'mobx-react';
import { types, flow, getEnv, IModelType, getSnapshot } from 'mobx-state-tree';
import * as keycode from 'keycode';
import Downshift from 'downshift';
import Debug from 'debug';

import { withStyles, WithStyles } from 'material-ui/styles';
import Input, { InputLabel, InputAdornment } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import { MenuItem } from 'material-ui/Menu';
import TextField from 'material-ui/TextField';
import Paper from 'material-ui/Paper';
import Chip from 'material-ui/Chip';
import IconButton from 'material-ui/IconButton';
import Close from '@material-ui/icons/Close';
import ArrowDown from '@material-ui/icons/KeyboardArrowDown';

import { models, inject_props } from '../../utils';

const debug = new Debug('selecter');

const renderSuggestion = ({ suggestion, index, itemProps, highlightedIndex, selectedItem }) => {
  const isHighlighted = highlightedIndex === index;
  const isSelected = selectedItem && selectedItem === suggestion[1];

  return (
    <MenuItem
      {...itemProps}
      key={suggestion[1]}
      selected={isHighlighted}
      component='div'
      style={{
        fontWeight: isSelected ? 'bold' : 'normal',
      }}
    >
      {suggestion[1]}
    </MenuItem>
  );
};

interface DownshiftProps {

}
interface DownshiftState {
  inputValue: string;
  selectedItem: string[];
}

interface SelectProps {
  id: string;
  required?: boolean;
  label: string;
  error?: any;
  type?: string;
  value?: any;
  onChange?: (newVal: any) => void;
  fieldProps?: any;

  projectionStore: IModelType<{}, {}>;
  projection: (store: any, term: string, id?: string) => Promise<{ id: string, label: string }[]>;
  select: (store: any, id: string) => any;
  getIdentity: (model: any) => string;
  addComponent?: React.ComponentType<{ onChange: (newVal: any) => void }>;

  // injected
  projectionStoreStore?: any;
  selectStore?: SelectType;
}

interface SelectType {
  selected?: { id: string, label: string };

  loading: boolean;
  term: string;
  records: Map<string, string>;
  list: (id?: string) => Promise<{}>;
  updateTerm: (newVal: string) => Promise<{}>;
  clear: () => void;
  itemToSelect: (id: string) => Promise<{}>;
  change: (item: { id: string, label: string }) => void;
}
const selectModel = types
  .model(
    {
      selected: types.maybe(types.model({
        id: types.string,
        label: types.string,
      })),
      loading: types.optional(types.boolean, false),
      term: types.optional(types.string, ''),
      records: types.optional(types.map(types.string), {})
    })
  .actions(self => {
    const list: (id?: string) => Promise<{}> = flow(function*(id?: string) {
      const projectionStore = getEnv(self).store;
      const projection = getEnv(self).projection as (store: any, term: string, id?: string) => Promise<{ id: string, label: string }[]>;

      self.loading = true;
      try {
        const records = yield projection(projectionStore, self.term);
        records.forEach((record) => {
          self.records.set(record.id, record.label);
        });
      } catch (e) {
        debug('error fetching: ', e);
      }
      self.loading = false;
    });
    const updateTerm = flow(function*(newVal: string) {
      self.term = newVal;
      yield list();
    });
    const clear = () => {
      self.records.clear();
      self.term = '';
    };
    const itemToSelect = flow(function*(id: string) {
      if (self.records.has(id)) {
        self.selected = { id, label: self.records.get(id) };
      }

      yield list(id);
      self.selected = { id, label: self.records.get(id) };
    });
    const change = (item: {id: string, label: string}) => {
      self.selected = item;
    };
    return { change, itemToSelect, list, updateTerm, clear };
  });

@inject((stores, props: SelectProps) => {
  props['projectionStoreStore'] = props['projectionStoreStore'] || props.projectionStore.create({}, { api: stores['store'].api });
  props['selectStore'] = props['selectStore'] || selectModel.create({}, { store: props['projectionStoreStore'], projection: props.projection });
  return props;
})
@observer
class IntegrationDownshift extends React.Component<SelectProps & WithStyles<'paper' | 'root' | 'container' | 'formControl'>, {}> {

  private _value: string;
  private _clearSelection: () => void;

  public componentDidMount() {
    const { value, selectStore, getIdentity } = this.props;
    if (value) {
      selectStore.itemToSelect(getIdentity(value));
    } else {
      selectStore.list();
    }
  }
  public componentDidUpdate() {
    const { value, getIdentity } = this.props;
    if (!value && this._value) {
      this._value = undefined;
      this._clearSelection();
    }
    const { selectStore } = this.props;
    selectStore.list();
  }

  public onFocus = () => {
    const { selectStore } = this.props;
    selectStore.list();
  }
  public onChange = (newVal: string) => {
    const { selectStore, onChange } = this.props;

    const label = this._value && selectStore.records.get(this._value);
    if (label !== newVal) {
      onChange(undefined);
    }

    selectStore.updateTerm(newVal);
  }
  public selectionChanged = (selection: string[]) => {
    const { selectStore, onChange, select, projectionStoreStore } = this.props;

    if (!selection) {
      this._value = undefined;
      onChange(undefined);
      return;
    }

    selectStore.change({ id: selection[0], label: selection[1] });
    onChange(select ? getSnapshot(select(projectionStoreStore, selection[0])) : selection[0]);
    selectStore.clear();
    this._value = selection[0];
  }
  public addAndSelect = (data: any) => {
    this.selectionChanged([data.id, data.brand]);
  }

  private _renderDownshift = (classes, error, required, label, id, fieldProps, selectStore, value, addComponent) => ({ getInputProps, getItemProps, isOpen, inputValue, selectedItem, highlightedIndex, clearSelection, openMenu }) => {
    this._clearSelection = clearSelection;
    return (
      <div className={classes.container}>

        <FormControl required={required} className={classes.formControl} error={error && error[id] ? true : false} aria-describedby={id + '-text'}>
          <InputLabel htmlFor={id}>{label}</InputLabel>
          <Input id={id} onChange={this.onChange} onFocus={this.onFocus} type='text' fullWidth {...fieldProps} {...getInputProps()} endAdornment={
            value ?
              <InputAdornment position='end'>
                <IconButton
                  aria-label='Clear'
                  onClick={clearSelection}
                >
                  <Close />
                </IconButton>
              </InputAdornment>
              :
              <InputAdornment position='end'>
                <IconButton
                  aria-label='Open'
                  onClick={openMenu}
                >
                  <ArrowDown />
                </IconButton>
                {addComponent && React.createElement(addComponent, { onChange: this.addAndSelect })}
              </InputAdornment>
          } />
          {error && error[id] ? error[id].map((e, key) => (<FormHelperText key={key} id={id + '-' + key + '-text'}>{e}</FormHelperText>)) : undefined}
        </FormControl>
        {isOpen ? (
          <Paper className={classes.paper} elevation={3} square>
            {Array.from(selectStore.records.entries()).map((suggestion, index) =>
              renderSuggestion({
                suggestion,
                index,
                itemProps: getItemProps({ item: suggestion }),
                highlightedIndex,
                selectedItem,
              }),
            )}
          </Paper>
        ) : null}
      </div>
    );
  }

  public render() {
    const { classes, error, required, label, id, fieldProps, selectStore, value, addComponent, onChange } = this.props;

    return (
      <div className={classes.root}>
        <Downshift selectedItem={ selectStore.selected && [selectStore.selected.id, selectStore.selected.label]} onChange={this.selectionChanged} itemToString={item => item && item[1]}>
          {this._renderDownshift(classes, error, required, label, id, fieldProps, selectStore, value, addComponent)}
        </Downshift>
      </div>
    );
  }
}

const styles = theme => ({
  root: {
    flexGrow: 1,
  },
  container: {
    flexGrow: 1,
    position: 'relative',
  },
  paper: {
    position: 'absolute',
    zIndex: 1,
    marginTop: theme.spacing.unit,
    left: 0,
    right: 0,
  }
});
export default withStyles(styles as any)<SelectProps>(IntegrationDownshift);
