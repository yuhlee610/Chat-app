const addHours = (date: Date, hours: number) => {
  return new Date(date.getTime() + hours * 60 * 60 * 1000);
};

const subtractHours = (date: Date, hours: number): Date => {
  return new Date(date.getTime() - hours * 60 * 60 * 1000);
};

export const toLocalDateTime = (date: string): Date => {
  const operator = date.slice(-6, -5);
  const hours = date.slice(-5, -3);
  if (operator === "+") {
    return addHours(new Date(date), hours as unknown as number);
  } else if (operator === "-") {
    return subtractHours(new Date(date), hours as unknown as number);
  }
  return addHours(new Date(date), hours as unknown as number);
};

export const renameKeys = (keysMap, obj) =>
  Object.keys(obj).reduce(
    (acc, key) => ({
      ...acc,
      ...{ [keysMap[key] || key]: obj[key] },
    }),
    {}
  );

export const generateKey = (keys: string[]): string => keys.sort().join("-");
