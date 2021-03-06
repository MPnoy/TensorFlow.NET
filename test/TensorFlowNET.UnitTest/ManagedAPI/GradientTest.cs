﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tensorflow;
using static Tensorflow.Binding;

namespace TensorFlowNET.UnitTest.ManagedAPI
{
    [TestClass]
    public class GradientTest
    {
        [TestMethod]
        public void GradientFloatTest()
        {
            var x = tf.Variable(3.0, dtype: tf.float32);
            using var tape = tf.GradientTape();
            var y = tf.square(x);
            var y_grad = tape.gradient(y, x);
            Assert.AreEqual(9.0f, (float)y);
        }

        [TestMethod]
        public void GradientDefaultTest()
        {
            var x = tf.Variable(3.0);
            using var tape = tf.GradientTape();
            var y = tf.square(x);
            var y_grad = tape.gradient(y, x);
            Assert.AreEqual(9.0, (double)y);
        }

        [TestMethod]
        public void GradientDoubleTest()
        {
            var x = tf.Variable(3.0, dtype: tf.float64);
            using var tape = tf.GradientTape();
            var y = tf.square(x);
            var y_grad = tape.gradient(y, x);
            Assert.AreEqual(9.0, (double)y);
        }

        [TestMethod]
        public void GradientOperatorMulTest()
        {
            var x = tf.constant(0f);
            var w = tf.Variable(new float[] { 1, 1 });
            using var gt = tf.GradientTape();
            var y = x * w;
            var gr = gt.gradient(y, w);
            Assert.AreEqual(new float[] { 0, 0 }, gr.numpy());
        }

        [TestMethod]
        public void GradientSliceTest()
        {
            var X = tf.zeros(new TensorShape(10));
            var W = tf.Variable(-0.06f, name: "weight");
            var b = tf.Variable(-0.73f, name: "bias");
            using var g = tf.GradientTape();
            var pred = W * X + b;
            var test = tf.slice(pred, new[] { 0 }, pred.shape);
            var gradients = g.gradient(test, (W, b));
            Assert.AreNotEqual(gradients.Item1, null);
            Assert.AreNotEqual(gradients.Item2, null);
        }

        [TestMethod]
        public void GradientConcatTest()
        {
            var X = tf.zeros(new TensorShape(10));
            var W = tf.Variable(-0.06f, name: "weight");
            var b = tf.Variable(-0.73f, name: "bias");
            using var g = tf.GradientTape();
            var test = tf.concat(new Tensor[] { W, b }, 0);
            var pred = test[0] * X + test[1];
            var gradients = g.gradient(pred, (W, b));
            Assert.AreEqual((float)gradients.Item1, 0);
            Assert.AreEqual((float)gradients.Item2, 10);
        }
    }
}
