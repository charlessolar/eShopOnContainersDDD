
export function toFixed(value: number, precision: number) {
  const multiplier = Math.pow(10, precision);
  return (Math.round(value * multiplier) / multiplier).toFixed(precision);
}

export function Round(value: number, precision: number) {
  const multiplier = Math.pow(10, precision);
  return (Math.round(value * multiplier) / multiplier);
}
