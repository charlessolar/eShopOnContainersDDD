export default {
  brand: {
    presence: true,
    length: {
      minimum: 3,
      message: 'must be at least 3 characters'
    }
  },
  type: {
    presence: true,
    length: {
      minimum: 3,
      message: 'must be at least 3 characters'
    }
  }
};
