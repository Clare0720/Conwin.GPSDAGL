// Exponential-Golomb buffer decoder
function ExpGolomb(uint8array) {
    this.TAG = 'ExpGolomb';

    this._buffer = uint8array;
    this._buffer_index = 0;
    this._total_bytes = uint8array.byteLength;
    this._total_bits = uint8array.byteLength * 8;
    this._current_word = 0;
    this._current_word_bits_left = 0;
}

ExpGolomb.prototype.destroy = function () {
    this._buffer = null;
}

ExpGolomb.prototype._fillCurrentWord = function () {
    let buffer_bytes_left = this._total_bytes - this._buffer_index;
    if (buffer_bytes_left <= 0)
        throw new IllegalStateException('ExpGolomb: _fillCurrentWord() but no bytes available');

    let bytes_read = Math.min(4, buffer_bytes_left);
    let word = new Uint8Array(4);
    word.set(this._buffer.subarray(this._buffer_index, this._buffer_index + bytes_read));
    this._current_word = new DataView(word.buffer).getUint32(0, false);

    this._buffer_index += bytes_read;
    this._current_word_bits_left = bytes_read * 8;
}

ExpGolomb.prototype.readBits = function (bits) {
    if (bits > 32)
        throw new InvalidArgumentException('ExpGolomb: readBits() bits exceeded max 32bits!');

    if (bits <= this._current_word_bits_left) {
        let result = this._current_word >>> (32 - bits);
        this._current_word <<= bits;
        this._current_word_bits_left -= bits;
        return result;
    }

    let result = this._current_word_bits_left ? this._current_word : 0;
    result = result >>> (32 - this._current_word_bits_left);
    let bits_need_left = bits - this._current_word_bits_left;

    this._fillCurrentWord();
    let bits_read_next = Math.min(bits_need_left, this._current_word_bits_left);

    let result2 = this._current_word >>> (32 - bits_read_next);
    this._current_word <<= bits_read_next;
    this._current_word_bits_left -= bits_read_next;

    result = (result << bits_read_next) | result2;
    return result;
}

ExpGolomb.prototype.readBool = function () {
    return this.readBits(1) === 1;
}

ExpGolomb.prototype.readByte = function () {
    return this.readBits(8);
}

ExpGolomb.prototype._skipLeadingZero = function () {
    let zero_count;
    for (zero_count = 0; zero_count < this._current_word_bits_left; zero_count++) {
        if (0 !== (this._current_word & (0x80000000 >>> zero_count))) {
            this._current_word <<= zero_count;
            this._current_word_bits_left -= zero_count;
            return zero_count;
        }
    }
    this._fillCurrentWord();
    return zero_count + this._skipLeadingZero();
}

ExpGolomb.prototype.readUEG = function () {  // unsigned exponential golomb
    let leading_zeros = this._skipLeadingZero();
    return this.readBits(leading_zeros + 1) - 1;
}

ExpGolomb.prototype.readSEG = function () {  // signed exponential golomb
    let value = this.readUEG();
    if (value & 0x01) {
        return (value + 1) >>> 1;
    } else {
        return -1 * (value >>> 1);
    }
}


