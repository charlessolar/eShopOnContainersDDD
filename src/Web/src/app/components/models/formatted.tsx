import * as React from 'react';
import { observer } from 'mobx-react';
import * as numeral from 'numeral';
import { DateTime } from 'luxon';

import { Typography } from 'material-ui';

import { toFixed, Round } from '../../utils/math';
import { UsingContext, UsingType } from './using';

interface FormattedProps {
  field: string;
  options?: any;
}

export class Formatted extends React.Component<FormattedProps, {}> {

  public render() {
    const { field, options } = this.props;

    return (
      <UsingContext.Consumer>
        {value => (
          <FormattedConsumer using={value} field={field} options={options} />
        )}
      </UsingContext.Consumer>
    );
  }
}

// need to wrap this in observer to make magic happen
const FormattedConsumer = observer((props: FormattedProps & { using: UsingType<any> }) => {

  const { using, field } = props;
  const definition = using.formatting && using.formatting[field] ? using.formatting[field] : {};

  let fieldValue = using.payload[field];

  if (typeof fieldValue !== 'number') {
    fieldValue = Number(fieldValue);
  }

  if (fieldValue === null || fieldValue === undefined || fieldValue === Number.NaN) {
    return (<>{definition.defaultValue || ''}</>);
  }

  if (definition.normalize) {
    fieldValue = fieldValue / Math.pow(10, definition.normalize);
  }

  if (definition.negative) {
    fieldValue *= -1;
  }
  if (definition.hideZero && fieldValue === 0) {
    return (<></>);
  }

  let format = '0';
  if (definition.trim) {
    format += '[.][000000]';
  } else if (!definition.precision || definition.precision > 0) {
    // creates a series of 0s depending on precision wanted (default 2)
    format += '.' + Array((definition.precision || 2) + 1).join('0');
  }
  if (!definition.commas || definition.commas === true) {
    format = '0,' + format;
  }
  if (definition.financial) {
    format = '(' + format + ')';
  }
  if (definition.percent) {
    format += '%';
  }
  if (definition.currency) {
    format = '$' + format;
  }

  const formatted = numeral(fieldValue).format(format);
  return (<>{formatted}</>);
});
